function validarRegistro() {
    const nombre = document.getElementById("nombre").value;
    const apellido = document.getElementById("apellido").value;
    const nombreUsuario = document.getElementById("nombreUsuario").value;
    const contrasena = document.getElementById("contrasena").value;
    const errorJs = document.getElementById("errorJs");

    if (nombre === "" || apellido === "" || nombreUsuario === "" || contrasena === "") {
        errorJs.innerHTML = "Por favor, completá todos los campos antes de registrarte.";
        return false;
    }

    if (contrasena.length < 4) {
        errorJs.innerHTML = "La contraseña debe tener al menos 4 caracteres.";
        return false;
    }

    errorJs.innerHTML = "";
    return true;
}
