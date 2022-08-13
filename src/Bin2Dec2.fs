﻿// taidalab Version 1.6.0
// https://github.com/taidalog/taidalab
// Copyright (c) 2022 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/taidalab/blob/main/LICENSE
namespace Taidalab

open System
open Browser.Dom
open Taidalab.Common

module Bin2Dec2 =

    let rec checkAnswer answer (question : string) last_answers =
        // Getting the user input.
        let numberInput = document.getElementById "numberInput" :?> Browser.Types.HTMLInputElement
        let inputValue = escapeHtml numberInput.value
        printfn "inputValue: %s" inputValue
        
        numberInput.focus()

        // Making an error message.
        let questionWithoutSpace = question.Replace(" ", "")
        let errorMessage = newErrorMessageDec questionWithoutSpace inputValue
        (document.getElementById "errorArea").innerHTML <- errorMessage
        
        // Exits when the input was invalid.
        if errorMessage <> "" then
            ()
        else
            
            let inputValueAsInt = int inputValue
            printfn "inputValueAsInt: %d" inputValueAsInt

            // Converting the input in order to use in the history message.
            let digit = 3
            let spacePaddedInputValue = inputValue |> padStart " " digit |> escapeSpace
            printfn "spacePaddedInputValue: %s" spacePaddedInputValue

            let sourceRadix = 2
            let bin = toBinary inputValueAsInt
            printfn "inputValue -> binary: %s" bin

            // Making a new history and updating the history with the new one.
            let destinationRadix = 10
            let outputArea = document.getElementById("outputArea")
            let historyMessage =
                newHistory (inputValueAsInt = answer) spacePaddedInputValue destinationRadix bin sourceRadix
                |> (fun x -> concatinateStrings "<br>" x outputArea.innerHTML)
            //printfn "historyMessage: %s" historyMessage
            outputArea.innerHTML <- historyMessage
            
            if inputValueAsInt = answer then
                // Making the next question.
                let mutable nextNumber = getRandomBetween 0 255
                
                printfn "last_answers: %A" last_answers
                while List.contains nextNumber last_answers do
                    nextNumber <- getRandomBetween 0 255
                    printfn "nextNumber: %d" nextNumber
                    printfn "List.contains nextNumber last_answers: %b" (List.contains nextNumber last_answers)

                let nextBin = toBinary nextNumber
                let splitBin = splitBinaryStringBy 4 nextBin
                printfn "nextBin: %s" nextBin
                printfn "splitBin: %s" splitBin
                
                (document.getElementById "questionSpan").innerText <- splitBin

                numberInput.value <- ""

                // Updating `lastAnswers`.
                // These numbers will not be used for the next question.
                let answersToKeep = Math.Min(10, List.length last_answers + 1)
                let lastAnswers = (nextNumber :: last_answers).[0..(answersToKeep - 1)]

                // Setting the next answer to the check button.
                (document.getElementById "submitButton").onclick <- (fun _ ->
                    checkAnswer nextNumber splitBin lastAnswers
                    false)


    let init () =
        // Initialization.
        let initNumber = getRandomBetween 0 255
        let initBin = toBinary initNumber
        let splitBin = splitBinaryStringBy 4 initBin
        printfn "initNumber %d" initNumber
        printfn "initBin %s" initBin
        printfn "splitBin %s" splitBin
        
        let sourceRadix = 2
        let destinationRadix = 10
        
        (document.getElementById "questionSpan").innerText <- splitBin
        (document.getElementById "binaryRadix").innerHTML <- sprintf "<sub>(%d)</sub>" destinationRadix
        (document.getElementById "srcRadix").innerHTML <- sprintf "(%d)" sourceRadix
        (document.getElementById "dstRadix").innerHTML <- string destinationRadix
        (document.getElementById "submitButton").onclick <- (fun _ ->
            checkAnswer initNumber splitBin [initNumber]
            false)