// taidalab Version 4.6.3
// https://github.com/taidalog/taidalab
// Copyright (c) 2022-2024 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/taidalab/blob/main/LICENSE
namespace Taidalab

open Browser.Dom
open Browser.Types
open Taidalab.Text
open Taidalab.EndlessBinary
open Fermata
open Fermata.RadixConversion

module NotFound =

    let rec checkAnswer answer =
        // Getting the user input.
        let numberInput = document.getElementById "numberInput" :?> HTMLInputElement
        let input = numberInput.value |> escapeHtml
        let bin: Result<string, Errors.Errors> = input |> Bin.validate

        numberInput.focus ()

        match bin with
        | Error(error: Errors.Errors) ->
            // Making an error message.
            (document.getElementById "errorArea").innerHTML <- newErrorMessageBin answer input error
        | Ok(bin: string) ->
            (document.getElementById "errorArea").innerHTML <- ""
            // Converting the input in order to use in the history message.
            let binaryDigit = 9
            let destinationRadix = 2
            let taggedBin = padWithZero binaryDigit bin |> colorLeadingZero
            let dec = Bin.toDec bin

            let decimalDigit = 3

            let spacePaddedDec =
                dec |> string |> Fermata.String.padLeft decimalDigit ' ' |> escapeSpace

            // Making a new history and updating the history with the new one.
            let sourceRadix = 10
            let outputArea = document.getElementById "outputArea" :?> HTMLParagraphElement

            let historyMessage =
                newHistory (dec = int answer) taggedBin destinationRadix spacePaddedDec sourceRadix
                |> (fun x -> concatinateStrings "<br>" [ x; outputArea.innerHTML ])

            outputArea.innerHTML <- historyMessage

            if dec <> int answer then
                ()
            else
                // Redirecting to the home.
                ("/taidalab/", Taidalab.Home.main, (fun _ -> ()))
                |||> InitObject.create
                |> Page.replace


    let init () =
        // Initialization.
        document.title <- "404: Page Not Found - taidalab"

        let header = document.querySelector "header"
        header.innerHTML <- Content.Common.headerNoHelp
        header.className <- "not-found"

        (document.getElementById "hamburgerButton").onclick <-
            (fun _ ->
                (document.querySelector "aside").classList.toggle "flagged" |> ignore
                (document.getElementById "barrier").classList.toggle "flagged" |> ignore
                (document.querySelector "main").classList.toggle "flagged" |> ignore)

        (document.getElementById "barrier").onclick <-
            (fun _ ->
                (document.querySelector "aside").classList.remove "flagged" |> ignore
                (document.getElementById "barrier").classList.remove "flagged" |> ignore
                (document.querySelector "main").classList.remove "flagged" |> ignore)

        (document.querySelector "#headerTitle").innerHTML <-
            """<h1>404: Page Not Found - <span translate="no">taidalab</span></h1>"""

        (document.querySelector "main").innerHTML <- EndlessBinary.Course.main404
        (document.querySelector "#submitButton").className <- "submit-button display-order-3 not-found"
        (document.querySelector "#questionArea").innerHTML <- Content.Common.question

        let initNumber = 404
        let sourceRadix = 10
        let destinationRadix = 2

        (document.getElementById "questionSpan").innerText <- string initNumber
        (document.getElementById "srcRadix").innerText <- sprintf "(%d)" sourceRadix
        (document.getElementById "dstRadix").innerText <- string destinationRadix
        (document.getElementById "binaryRadix").innerHTML <- sprintf "<sub>(%d)</sub>" destinationRadix

        (document.getElementById "submitButton").onclick <-
            (fun e ->
                e.preventDefault ()
                checkAnswer (string initNumber))

        (document.getElementById "inputArea").onsubmit <-
            (fun e ->
                e.preventDefault ()
                checkAnswer (string initNumber))
