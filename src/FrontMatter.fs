module FrontMatter

open System
open Legivel.Attributes

type Options = {
    [<YamlField("namespace")>] CodeNamespace: string option
}


let splitDoc (text: string) (marker: string) = 
    let marker = marker + Environment.NewLine
    let index = text.IndexOf(marker, marker.Length)
    let fstart, fend, docstart = 
        if index < 0 then 
            (0, 0, 0) 
        else 
            (marker.Length, index - marker.Length, index + marker.Length)
    text.Substring (fstart, fend), text.Substring(docstart)

