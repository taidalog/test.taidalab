// taidalab Version 1.4.0
// https://github.com/taidalog/taidalab
// Copyright (c) 2022 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/taidalab/blob/main/LICENSE
function checkAnswerd2b2 (answer, last_answers) {
    const numberInput = document.getElementById('numberInput');
    const bin = escapeHtml(numberInput.value);
    console.log(bin);
    
    numberInput.focus();
    
    const errorMessage = newErrorMessageBin(answer, bin);
    const errorArea = document.getElementById('errorArea');
    errorArea.innerHTML = errorMessage;
    
    if (errorMessage) {
        return;
    }
    
    const binaryDigit = 8;
    const destinationRadix = 2;
    const zeroPaddedBin = bin.padStart(binaryDigit, '0');
    const taggedBin = colorLeadingZero(zeroPaddedBin);
    const dec = parseInt(bin, destinationRadix);
    console.log(taggedBin);
    console.log(dec);
    
    const decimalDigit = 3;
    const spacePaddedDec = dec.toString().padStart(decimalDigit, ' ').replace(' ', '&nbsp;');
    
    const sourceRadix = 10;
    const outputArea = document.getElementById('outputArea');
    const currentHistoryMessage = newHistory((dec == answer), taggedBin, destinationRadix, spacePaddedDec, sourceRadix);
    const historyMessage = concatinateStrings(currentHistoryMessage, outputArea.innerHTML);
    console.log(currentHistoryMessage);
    console.log(historyMessage);
    outputArea.innerHTML = historyMessage;
    
    if (dec == answer) {
        let nextNumber = 0;

        console.log(last_answers);
        do {
            nextNumber = getRandomBetween(0, 255);
            console.log(nextNumber);
            console.log(last_answers.some((element) => element == nextNumber));
        } while (last_answers.some((element) => element == nextNumber));

        document.getElementById('questionSpan').innerText = nextNumber;
        numberInput.value = '';

        const answersToKeep = 10;
        const lastAnswers = [nextNumber].concat(last_answers).slice(0, answersToKeep);
        document.getElementById('submitButton').onclick = function() { checkAnswerd2b2(nextNumber, lastAnswers); return false; };
    }
}


function initDec2Bin2 () {
    // initialization
    const initNumber = getRandomBetween(0, 255);
    const sourceRadix = 10;
    const destinationRadix = 2;
    
    document.getElementById('questionSpan').innerHTML = initNumber;
    document.getElementById('srcRadix').innerHTML = '(' + sourceRadix + ')';
    document.getElementById('dstRadix').innerHTML = destinationRadix;
    document.getElementById('binaryRadix').innerHTML = '<sub>(' + destinationRadix + ')</sub>';
    document.getElementById('submitButton').onclick = function() { checkAnswerd2b2(initNumber, [initNumber]); return false; };
}
