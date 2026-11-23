let reservaValida = false;

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
