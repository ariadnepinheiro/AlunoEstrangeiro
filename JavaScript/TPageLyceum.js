// -----------------------------------------------------------------------------
// Customizar a função waitSign() de acordo com o layout do sistema em questão.
// -----------------------------------------------------------------------------
function waitSign() {
  var table = document.createElement("table");
  var row = table.insertRow();
  var cell = row.insertCell();
  
  table.width = "100%";
  cell.valign = "middle";
  
  var spam = document.createElement(
    "<spam style='" +
      "background-color:white;" +
      "z-index:500;" +
      "border-style:solid;" +
      "border-width:1px;" +
      "border-color:black;" +
      "position:absolute;" +
      "text-align:center;" +
      "vertical-align:middle;" +
      "top:10;" +
      "left:10;" +
      "height:250px;" +
      "width:90%" +
    "'>"
  );
  
  spam.innerHTML = "Aguarde...";
  cell.insertBefore(spam);
  
  document.body.insertBefore(table);
}
