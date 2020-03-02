open System
open FSharp.Markdown
open System.IO
open System.Text.RegularExpressions
open System.Globalization
open FSharp.Formatting.Common
open System.Linq


[<EntryPoint>]
let main argv =    
    
    let text = File.ReadAllText("Sample.md")
    let frontmatter, doc = FrontMatter.splitDoc text "---"
    
    let parsed = Markdown.Parse(doc)    
    
    //let test p str =
    //    match run p str with
    //    | Success(result, _, _)   -> printfn "Success: %A" result
    //    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

    //test pint32 "0Ah"
    parsed.Paragraphs 
    |> Seq.iter (function
                | Heading(2, [Literal(text, _)], range) as hdg -> Parsers.parseRegisters text range |> printfn "%A"
                //| par -> printfn "%A" par
                | _ -> ()
                )
    
    0 // return an integer exit code
