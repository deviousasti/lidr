module Parsers

open Ast
open System.Text.RegularExpressions
open System
open System.Globalization
open FSharp.Formatting.Common


let private registerRegex = Regex("(?<dir>(R|W|RW)\:)?(?<name>([A-Z][A-Z_0-9]+))\s*\((0x)?(?<hex>[A-F0-9]+)(h?)\)")    

let parseHex shex = 
    let result = ref 0UL
    if UInt64.TryParse(shex, NumberStyles.HexNumber, null, result) then 
        !result
    else
        UInt64.MaxValue

let isInvalid = function | Invalid _ -> true | _ -> false
let isBadAddress = function | UInt64.MaxValue -> true | _ -> false
 
let addr  (reg: RegisterDefinition) = reg.address
let label (reg: RegisterDefinition) = reg.label

let (|IsContiguous|_|) regs = 
    regs 
    |> Seq.map addr
    |> Seq.pairwise 
    |> Seq.map (fun (a1, a2) -> a2 - a1)
    |> Seq.forall ((=) 1UL)
    |> function | true -> Some(regs) | _ -> None

let (|HasBadAddress|_|) regs = 
    regs |> List.exists(addr >> isBadAddress) |> function | true -> Some(regs) | _ -> None
     
let parseDirection dir =
    (if String.IsNullOrEmpty(dir) then Direction.ReadWrite else Direction.None)     |||
    (if dir |> String.exists ((=) 'R') then Direction.Read  else Direction.None)    |||
    (if dir |> String.exists ((=) 'W') then Direction.Write else Direction.None)

let trim label = (string label).Trim('_')
let findCommonName (names: string list) =             
        let smallest = names |> Seq.map (String.length) |> Seq.min
        [0..smallest] 
        |> Seq.map (fun n -> names |> Seq.map(fun name -> name.Substring(0, n)))
        |> Seq.takeWhile (fun all -> all |> Seq.distinct |> Seq.length = 1)
        |> Seq.last
        |> Seq.head
        |> trim

let parseRegisters text range =            
    registerRegex.Matches(text)
    |> Seq.cast<Match>
    |> Seq.map (fun m ->                 
                { 
                    label = m.Groups.["name"].Value 
                    address = parseHex m.Groups.["hex"].Value 
                    direction = parseDirection m.Groups.["dir"].Value
                    source = range
                }
        )
    |> Seq.sortBy (fun r -> r.address)
    |> Seq.toList
    |> function 
    | []    -> NotARegister(text, range)
    | [r]   -> ByteField r
    | HasBadAddress _ -> Invalid(Reason("Bad address"), text, range)
    | IsContiguous lst ->
        let cname = lst |> List.map(label) |> findCommonName                     
        Wide(cname, lst)
    | _ -> Invalid(Reason("Address range not valid or contiguous"), text, range)