// taidalab Version 4.1.0
// https://github.com/taidalog/taidalab
// Copyright (c) 2022-2023 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/taidalab/blob/main/LICENSE
namespace Taidalab

open Browser.Dom
open Browser.Types
open Fermata

[<StructuredFormatDisplay("{DisplayText}")>]
type Cable =
    { Id : string
      Kind : Kind
      Name : string
      Points : Point list
      Area : Area
      Position : Point }
    member this.DisplayText = this.ToString()
    override this.ToString() =
        sprintf "Id = %s; Kind = %s; Name = %s; Points = %s; Area = %O; Posirion = %O"
            this.Id (string this.Kind) this.Name (this.Points |> List.map (fun x -> x.ToString()) |> String.concat " ") this.Area this.Position

module Cable =
    let create id kind name points area position : Cable =
        { Cable.Id = id
          Cable.Kind = kind
          Cable.Name = name
          Cable.Points = points
          Cable.Area = area
          Cable.Position = position }
    
    let ofHTMLElement (elm: HTMLElement) : Cable option =
        let id = elm.id
        
        let name = document.getElementById(id + "Name").innerText
        
        let kind =
            document.getElementById(id + "Kind").innerText
            |> Kind.ofString
        
        let area =
            let svg = document.getElementById(id + "Svg")
            let rect = svg.getBoundingClientRect()
            Area.ofFloats rect.left rect.top rect.width rect.height
        
        let points =
            document.getElementById(id + "Polyline")
            |> fun (x: HTMLElement) -> x.getAttribute("points")
            |> String.split ' '
            |> List.map Point.ofString
        
        let position =
            let x =
                elm.getAttribute("style")
                |> Regex.match' """left: (\d+\.?\d+)px;"""
                |> fun m -> (m.Groups.Item 1).Value
                |> float
            let y =
                elm.getAttribute("style")
                |> Regex.match' """top: (\d+\.?\d+)px;"""
                |> fun m -> (m.Groups.Item 1).Value
                |> float
            Point.ofFloats x y

        match kind with
        | Some x -> Some (create id x name points area position)
        | None -> None
    
    let toHTMLElement (cable: Cable) : HTMLElement =
        let container = document.createElement("div")
        container.id <- cable.Id
        container.className <- "device cable-container lan-cable"
        container.setAttribute("style", $"top: %f{cable.Position.Y}px; left: %f{cable.Position.X}px;")

        let svg = document.createElementNS("http://www.w3.org/2000/svg", "svg")
        svg.id <- $"%s{cable.Id}Svg"
        svg.classList.add("device")
        svg.classList.add("device")
        svg.setAttribute("viewBox", $"%f{cable.Area.X} %f{cable.Area.Y} %f{cable.Area.Width} %f{cable.Area.Height}")
        svg.setAttribute("width", $"%f{cable.Area.Width}px")
        svg.setAttribute("height", $"%f{cable.Area.Height}px")

        let g = document.createElementNS("http://www.w3.org/2000/svg", "g")

        let title = document.createElementNS("http://www.w3.org/2000/svg", "title")
        title.id <- $"%s{cable.Id}Title"
        title.textContent <- $"%s{cable.Name}"

        let polyline = document.createElementNS("http://www.w3.org/2000/svg", "polyline")
        polyline.id <- $"{cable.Id}Polyline"
        polyline.setAttribute("points", $"""%s{cable.Points |> List.map Point.toCoordinate |> String.concat " "}""")
        polyline.setAttribute("fill", "none")
        polyline.setAttribute("stroke", "#00aeda")
        polyline.setAttribute("stroke-width", "5")

        g.appendChild(title) |> ignore
        g.appendChild(polyline) |> ignore

        svg.appendChild(g) |> ignore

        let br1 = document.createElement("br")

        let spanName = document.createElement("span")
        spanName.id <- $"%s{cable.Id}Name"
        spanName.className <- "no-display"
        spanName.textContent <- $"%s{cable.Name}"

        let br2 = document.createElement("br")

        let spanKind = document.createElement("span")
        spanKind.id <- $"%s{cable.Id}Kind"
        spanKind.className <- "no-display"
        spanKind.textContent <- $"%s{cable.Kind |> string}"
        
        container.appendChild(svg) |> ignore
        container.appendChild(br1) |> ignore
        container.appendChild(spanName) |> ignore
        container.appendChild(br2) |> ignore
        container.appendChild(spanKind) |> ignore

        container
    
    let connectedTo (device: Device) (cable: Cable) : bool =
        cable.Points
        |> List.map (Point.shift cable.Area.X cable.Area.Y)
        |> List.exists (Area.includesPoint (Device.area device))