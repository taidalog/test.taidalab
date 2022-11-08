// taidalab Version 3.1.0
// https://github.com/taidalog/taidalab
// Copyright (c) 2022 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/taidalab/blob/main/LICENSE
namespace Taidalab

open Fable.Core
open Fable.Core.JsInterop
open Browser.Dom
open Taidalab.Switcher

module Main =

    window.addEventListener("DOMContentLoaded", (fun _ ->
        printfn "%s" "The begining of DOMContentLoaded"
        document.body.innerHTML <- """<div class="body-container"><div id="barrier" class="barrier"></div><header></header><div class="main-container"><aside></aside><main></main></div><footer></footer></div>"""
        (document.querySelector "footer").innerHTML <- Content.Common.footer
        (document.querySelector "aside").innerHTML <- Content.Common.aside
        printfn "%s" window.location.pathname
        let initObj = newInitObject window.location.pathname
        initPage initObj |> ignore

        let switchAnchorAction pathname (anchor : Browser.Types.HTMLAnchorElement) =
            (pathname, anchor.href, anchor)
            |> (fun (p, h, a) -> (p <> "/404/", isInnerPage h, a))
            |> (fun (p, h, a) ->
                match (p, h, a) with
                | (true, true, a) ->
                    (fun _ ->
                        overwriteAnchorClick
                            (fun _ ->
                                pushPage a.pathname
                                (document.querySelector "aside").classList.remove "active" |> ignore
                                (document.getElementById "barrier").classList.remove "active" |> ignore)
                            a)
                | (true, false, a) ->
                    (fun _ ->
                        ())
                | (false, true, a) ->
                    (fun _ ->
                        overwriteAnchorClick
                            (fun _ ->
                                replacePage a.pathname
                                (document.querySelector "aside").classList.remove "active" |> ignore
                                (document.getElementById "barrier").classList.remove "active" |> ignore)
                                a)
                | (false, false, a) ->
                    (fun _ ->
                        overwriteAnchorClick
                            (fun _ ->
                                window.location.replace a.pathname
                                (document.querySelector "aside").classList.remove "active" |> ignore
                                (document.getElementById "barrier").classList.remove "active" |> ignore)
                                a))
            |> (fun f -> f())

        (document.querySelector "aside").getElementsByTagName "a"
        |> (fun x -> JS.Constructors.Array?from(x))
        |> Array.toList
        |> List.map (switchAnchorAction initObj.pathname)
        |> ignore

        printfn "%s" "The end of DOMContentLoaded"
    ))

    window.addEventListener("popstate", (fun _ ->
        newInitObject window.location.pathname |> initPage
    ))
