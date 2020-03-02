module Ast

open System.Text.RegularExpressions
open System
open System.Globalization
open FSharp.Formatting.Common

[<Flags>]
 type Direction = None = 0 | Read = 1 | Write = 2 | ReadWrite = 3
 type Source = MarkdownRange option
 type RegisterDefinition =
     {
         label: string
         address: uint64
         direction: Direction
         source: Source
     }    

 type Reason = Reason of string

 type Register = 
     | NotARegister of string * Source
     | Invalid of Reason * string * Source
     | ByteField of RegisterDefinition 
     | BitField of RegisterDefinition
     | Wide of string * RegisterDefinition list

