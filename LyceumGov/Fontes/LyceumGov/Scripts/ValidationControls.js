function SomenteLetras(e) {
	if (document.all) { var evt = event.keyCode; }
	else { var evt = e.charCode; }
	var chr = String.fromCharCode(evt);
	var re = /[A-Za-z\s-ÃÕÑÁÉÍÓÚÀÜÇãõñáéíóúàçü]/;
	return (re.test(chr) || evt < 20); 
}