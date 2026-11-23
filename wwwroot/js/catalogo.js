let cantidadMostrada = 8;

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
