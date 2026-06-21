// JScript source code
/*****************Globals****************/
var currentText = new Array();
var pasteMsg = '';
/****************************************/

function startPreventPaste(objid, msg){
if(document.all){
    //it it is ie do the onpaste function
    var brwsr = navigator.userAgent.toLowerCase();
    if(brwsr.search(/opera[\/\s](\d+(\.?\d)*)/) != -1) {
    // Opera
    doLoop();
    }else{
for(i = 0; i < textArea.length; i++)
{
textArea.onpaste = function (){
showMessage();//shoe the message
return false;//don't paste
}
}
}
}else{
doLoop();
}
}
/****************************************************************************
void doLoop()
starts the timer. Checks the text.
****************************************************************************/
function doLoop(){
checkText();
pccpTimer = window.setTimeout("doLoop();", 10);
}
/****************************************************************************
void checkText()
checks the length of the string in the textbox and compares it with
the length of the string in the saved currentText string. If the
text box text is 10 characters longer than the saved string then it
assumes that they have pasted. it puts the current string back into
the textarea over what they pasted and then shows them the message.
if it isn't longer then it just puts the text into the saved text.
****************************************************************************/
function checkText(){
for(i = 0; i < textArea.length; i++)
{
if(textArea){
newTextLength = textArea.value.length;//gets length of the textarea right now.
currentTextLength = currentText.length;//gets length of the saved text from the textarea
if(newTextLength > (currentTextLength + 50)){//is the new more then 10 characters longer?
textArea.value = currentText;//put the saved text back in/
showMessage();//tell them they cannot paste.
}else{
currentText = textArea.value;//if it is ok then save the text.
}
}
}
}
/****************************************************************************
void showMessage();
if they calling function wants to show a message when they paste then
the message is alerted.
****************************************************************************/
function showMessage(){
if(pasteMsg !== false){
alert(pasteMsg);
}
}
===============================