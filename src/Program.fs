open System
open FSharp.Markdown
open System.IO
open Legivel.Attributes

type Options = {
    [<YamlField("namespace")>] CodeNamespace: string option
}

module FrontMatter = 
    let splitDoc (text: string) (marker: string) = 
        let index = text.IndexOf(marker, 4)
        let fstart, fend, docstart = 
            if index < 0 then 
                (0, 0, 0) 
            else 
                (marker.Length, index - marker.Length, index + marker.Length)
        text.Substring (fstart, fend), text.Substring(docstart)

[<EntryPoint>]
let main argv =
    
    let text = File.ReadAllText("Sample.md")
    let frontmatter, doc = FrontMatter.splitDoc text "---"
    let result = Legivel.Serialization.Deserialize<Options> frontmatter 
    let parsed = Markdown.Parse(doc)
    0 // return an integer exit code
