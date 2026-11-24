let cantidadMostrada = 8;
let reservaValida = false;

function validarRegistro() {
    const nombreUsuario = document.getElementById("nombreUsuario").value.trim();
    const contrasena = document.getElementById("contrasena").value;
    const nombre = document.getElementById("nombre").value.trim();
    const apellido = document.getElementById("apellido").value.trim();
    const mensajeError = document.getElementById("mensajeError");

    if (nombreUsuario === "" || contrasena === "" || nombre === "" || apellido === "") {
        mensajeError.innerHTML = "Todos los campos son obligatorios.";
        return false;
    }

    if (contrasena.length < 8) {
        mensajeError.innerHTML = "La contraseña debe tener al menos 8 caracteres.";
        return false;
    }

    const caracteresEspeciales = "!@#$%^&*()_+-=[]{};':\"\\|,.<>/?";
    let tieneEspecial = false;

    for (let i = 0; i < contrasena.length; i++) {
        if (caracteresEspeciales.includes(contrasena[i])) {
            tieneEspecial = true;
            break;
        }
    }

    if (!tieneEspecial) {
        mensajeError.innerHTML = "La contraseña debe incluir al menos un carácter especial.";
        return false;
    }

    return true;
}

function cargarMasEquipos() {
    fetch("/Home/ObtenerMasEquipos?saltar=" + cantidadMostrada)
        .then(function (response) {
            return response.json();
        })
        .then(function (data) {
            const grilla = document.getElementById("grillaEquipos");

            for (let i = 0; i < data.equipos.length; i++) {
                const eq = data.equipos[i];
                grilla.innerHTML +=
                    '<article class="tarjeta-equipo">' +
                        '<a href="/Home/Detalle?id=' + eq.id + '">' +
                            '<img src="/imagenes/' + eq.imagen + '" alt="' + eq.nombre + '" />' +
                            '<h3>' + eq.nombre + '</h3>' +
                            '<p>' + eq.nombreCategoria + '</p>' +
                        '</a>' +
                    '</article>';
            }

            cantidadMostrada += data.equipos.length;

            if (data.hayMas === false) {
                const btnVerMas = document.getElementById("btnVerMas");
                btnVerMas.style.display = "none";
            }
        });
}

function calcularConFetch() {
    const idEquipo = document.getElementById("idEquipo").value;
    const fechaInicio = document.getElementById("fechaInicio").value;
    const fechaFin = document.getElementById("fechaFin").value;
    const mensajeCalculo = document.getElementById("mensajeCalculo");
    const textoDias = document.getElementById("textoDias");
    const textoTotal = document.getElementById("textoTotal");

    reservaValida = false;
    textoDias.innerHTML = "";
    textoTotal.innerHTML = "";

    if (fechaInicio === "" || fechaFin === "") {
        mensajeCalculo.innerHTML = "";
        return;
    }

    fetch("/Home/CalcularAlquiler?idEquipo=" + idEquipo + "&fechaInicio=" + fechaInicio + "&fechaFin=" + fechaFin)
        .then(function (response) {
            return response.json();
        })
        .then(function (data) {
            if (data.valido === false) {
                mensajeCalculo.innerHTML = data.mensaje;
                reservaValida = false;
            } else {
                mensajeCalculo.innerHTML = "";
                textoDias.innerHTML = "Cantidad de días: " + data.dias;
                textoTotal.innerHTML = "Total a pagar: $" + data.total;
                reservaValida = true;
            }
        });
}

function validarFormularioReserva() {
    const fechaInicio = document.getElementById("fechaInicio").value;
    const fechaFin = document.getElementById("fechaFin").value;
    const mensajeCalculo = document.getElementById("mensajeCalculo");

    if (fechaInicio === "" || fechaFin === "") {
        mensajeCalculo.innerHTML = "Completá ambas fechas antes de confirmar.";
        return false;
    }

    if (reservaValida === false) {
        return false;
    }

    return true;
}
