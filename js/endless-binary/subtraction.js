// taidalab Version 1.1.0
// https://github.com/taidalog/taidalab
// Copyright (c) 2022 taidalog
// This software is licensed under the MIT License.
// https://github.com/taidalog/taidalab/blob/main/LICENSE
function checkAnswerSub(answer, num1, num2, last_answers) {
    
    const numberInput = document.getElementById('numberInput');
    const bin = escapeHtml(numberInput.value);
    console.log(bin);
    
    const errorArea = document.getElementById('errorArea');
    errorArea.innerHTML = '';
    
    const sourceRadix = 2;
    if (bin == '') {
        errorArea.innerHTML = '<span class="warning">' + num1.toString(sourceRadix) + '<small>(' + sourceRadix + ')</small>' + num2.toString(sourceRadix)+ '<small>(' + sourceRadix + ')</small>' + ' の2進法表記を入力してください。</span>';
    } else if (testBinaryString(bin) == false) {
        errorArea.innerHTML = '<span class="warning">"' + bin + '" は2進数ではありません。使えるのは半角の 0 と 1 のみです。</span>';
    } else {
        
        const digit = 8;
        const zeroPaddedBin = bin.padStart(digit, '0');
        const taggedBin = colorLeadingZero(zeroPaddedBin);
        const dec = parseInt(bin, sourceRadix);
        console.log(taggedBin);
        console.log(dec);
        
        const outputArea = document.getElementById('outputArea');
        
        let historyClassName = ''
        if (dec == answer) {
            historyClassName = 'history-correct'
        } else {
            historyClassName = 'history-wrong'
        }
        
        const destinationRadix = 10;
        const msg1 = '<span class ="' + historyClassName + '">' + taggedBin + '<sub>(' + sourceRadix + ')</sub> = ' + dec + '<sub>(' + destinationRadix + ')</sub></span>';
        const msg2 = concatinateStrings(msg1, outputArea.innerHTML);
        outputArea.innerHTML = msg2;
        console.log(msg1);
        console.log(msg2);
        
        if (dec == answer) {
            // Making next question.
            let numbers = [];

            console.log(last_answers);
            do {
                numbers = newNumbersSub();
                console.log(numbers[0]);
                console.log(numbers[1]);
                console.log(last_answers.some((element) => element == numbers[0] || element == numbers[1]));
            } while (last_answers.some((element) => element == numbers[0] || element == numbers[1]));

            console.log(numbers[0]);
            console.log(numbers[1]);
            console.log(numbers[0] - numbers[1]);
            console.log((numbers[0] - numbers[1]).toString(sourceRadix));
            setColumnAddition(numbers[0], numbers[1]);

            const hintArea = document.getElementById('hintArea');
            const nextHint = newHintSub();
            hintArea.innerHTML = nextHint;
            console.log(nextHint);

            numberInput.value = '';

            const answersToKeep = 20;
            const lastAnswers = [numbers[0], numbers[1]].concat(last_answers).slice(0, answersToKeep);
            document.getElementById('submitButton').onclick = function () { checkAnswerSub((numbers[0] - numbers[1]), numbers[0], numbers[1], lastAnswers); return false; };
        }
    }
    
    numberInput.focus();
}


function newNumbersSub () {
    const number1 = getRandomBetween(1, 255);
    console.log('number1: ' + number1);

    let number2 = 0;

    do {
        number2 = getRandomBetween(1, 255);
        console.debug('number1 == number2: ' + (number1 <= number2) + '\t(number1 & number2) == 0: ' + ((number1 & number2) == 0));
    } while ((number1 == number2) || ((number1 & number2) == 0))

    console.log(number2);

    if (number1 > number2) {
        return [number1, number2];
    } else {
        return [number2, number1];
    }
}


function newHintSub () {
    const hintFormat01 = '<details><summary>ヒント: </summary>';
    const hintFormat02 = '<p class="history-indented">';
    const hintFormat03 = '10進数の筆算と同じように、右端から上下の数で引き算をします。<br><br>';
    const hintFormat04 = '0<sub>(2)</sub> - 0<sub>(2)</sub> = 0<sub>(2)</sub><br>';
    const hintFormat05 = '1<sub>(2)</sub> - 1<sub>(2)</sub> = 0<sub>(2)</sub><br>';
    const hintFormat06 = '1<sub>(2)</sub> - 0<sub>(2)</sub> = 1<sub>(2)</sub><br><br>';
    const hintFormat07 = '0<sub>(2)</sub> - 1<sub>(2)</sub> をする時は、<br>';
    const hintFormat08 = 'ひとつ左の桁から1を2つもらってきます。<br>';
    const hintFormat09 = '</p>';
    const hintFormat10 = '</details>';
    const hint = hintFormat01 + hintFormat02 + hintFormat03 + hintFormat04 + hintFormat05 + hintFormat06 + hintFormat07 + hintFormat08 + hintFormat09 + hintFormat10;
    return hint;
}


function initSubtraction () {
    // initialization
    const sourceRadix = 2;
    const destinationRadix = 2;
    const hint = newHintSub();

//    document.title = '減算 - taidalab';
//    document.getElementsByTagName('header')[0].innerHTML = headerContentPages;
//    document.getElementsByTagName('header')[0].className = 'sub-header';
//    document.getElementById('headerContainer').innerHTML = '<h1>減算</h1>';
//    document.getElementsByTagName('main')[0].innerHTML = mainContentPages;
//    document.getElementById('submitButton').className = 'submit-button sub-button';
//    document.getElementById('questionArea').innerHTML = columnAdditionFormat;
//    document.getElementsByTagName('footer')[0].innerHTML = footerContentPages;
//    document.getElementById('versionNumber').innerText = 'Version 0.1.1';
    
    document.getElementById('numberInput').className = 'number-input question-number eight-digit';
    document.getElementById('operator').innerText = '-)';
    document.getElementById('firstRowSrcRadix').innerHTML = '(' + sourceRadix + ')';
    document.getElementById('secondRowSrcRadix').innerHTML = '(' + sourceRadix + ')';
    document.getElementById('binaryRadix').innerHTML = '<sub>(' + destinationRadix + ')</sub>';
    document.getElementById('hintArea').innerHTML = hint;

    const numbers = newNumbersSub();
    console.log(numbers[0]);
    console.log(numbers[1]);
    console.log(numbers[0] - numbers[1]);
    console.log((numbers[0] - numbers[1]).toString(sourceRadix));
    setColumnAddition(numbers[0], numbers[1]);

    document.getElementById('submitButton').onclick = function () { checkAnswerSub((numbers[0] - numbers[1]), numbers[0], numbers[1], [numbers[0], numbers[1]]); return false; };
}
