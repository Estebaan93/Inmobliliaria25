document.addEventListener("DOMContentLoaded", function () {
    const selEstado = document.getElementById("selectFiltroEstado");
    const selPropietario = document.getElementById("selectFiltroPorPropietario");
    const tabla = document.getElementById("tablaInmuebles");

    function cargarInmuebles() {
        const estado = selEstado.value; 
        const propietarioId = selPropietario.value;

        fetch(`/Inmueble/Filtrar?estado=${estado}&idPropietario=${propietarioId}`)
            .then(res => res.json())
            .then(data => {
                tabla.innerHTML = "";
                if (data.length === 0) {
                    tabla.innerHTML = `<tr><td colspan="9" class="text-center">No se encontraron inmuebles.</td></tr>`;
                    return;
                }

                data.forEach(item => {
                    let estadoTexto = item.estado === 1 ? "Disponible" :
                                      item.estado === 0 ? "No disponible" : "Alquilado";

                    tabla.innerHTML += `
                        <tr>
                            <td>${item.tipo.observacion}</td>
                            <td>${item.descripcion}</td>
                            <td>${item.cantidadAmbientes}</td>
                            <td>$${item.precio}</td>
                            <td>${item.cochera ? "Sí" : "No"}</td>
                            <td>${item.piscina ? "Sí" : "No"}</td>
                            <td>${item.mascotas ? "Sí" : "No"}</td>
                            <td>${estadoTexto}</td>
                            <td>
                                <a href="/Inmueble/Editar/${item.idInmueble}" class="btn btn-editar">Editar</a>
                                <form action="/Inmueble/Borrar" method="post" style="display:inline;">
                                    <input type="hidden" name="id" value="${item.idInmueble}" />
                                    <input type="submit" value="Eliminar" class="btn btn-eliminar" />
                                </form>
                            </td>
                        </tr>`;
                });
            })
            .catch(err => console.error("Error al cargar inmuebles:", err));
    }

    selEstado.addEventListener("change", cargarInmuebles);
    selPropietario.addEventListener("change", cargarInmuebles);

    cargarInmuebles();
});
