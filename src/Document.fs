module Document

open FSharp.Markdown

type DefinitionBlock = MarkdownParagraph * MarkdownParagraphs
type DocumentSection = MarkdownParagraph * DefinitionBlock seq

let scan (section, block, previous) current = 
    match current with 
    | Heading(size = 1) as h1 -> (h1, h1, current) 
    | Heading(size = 2) as h2 -> (section, h2, current) 
    | _ -> (section, block, current) 

//Group multiple sections and blocks spread across the document into one
let sectionize paragraphs = 
    let first = paragraphs |> List.head
    paragraphs 
    |> Seq.scan scan (first, first, first) 
    |> Seq.groupBy (fun (section, _, _) -> section)
    |> Seq.map (fun (section, contents) -> 
        section, contents 
        |> Seq.groupBy(fun (_, block, _) -> block)
        |> Seq.map (fun (block, contents) -> 
            block, contents 
            |> Seq.map (fun (_, _, content) -> content)
            |> Seq.toList                
        ) 
        |> Seq.map (DefinitionBlock)
    )
    |> Seq.map (DocumentSection)



