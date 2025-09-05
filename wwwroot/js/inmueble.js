//wwwroot/js/inmueble.js
document.addEventListener("DOMContentLoaded", function () {
	const selEstado = document.getElementById("selectFiltroEstado");
	const selPropietario = document.getElementById("selectFiltroPorPropietario");
	const tabla = document.getElementById("tablaInmuebles");

	//  FUNCION global dentro del scope del DOMContentLoaded
	function cargarInmuebles() {
		const estado = selEstado.value;
		const propietarioId = selPropietario.value;

		fetch(`/Inmueble/Filtrar?estado=${estado}&idPropietario=${propietarioId}`)
			.then((res) => res.json())
			.then((data) => {
				tabla.innerHTML = "";
				if (data.length === 0) {
					tabla.innerHTML = `<tr><td colspan="9" class="text-center">No se encontraron inmuebles.</td></tr>`;
					return;
				}

				data.forEach((item) => {
					let estadoTexto =
						item.estado === 1
							? "Disponible"
							: item.estado === 0
								? "No disponible"
								: "Alquilado";

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
                                <div class="d-flex gap-2">
                                    <a href="/Inmueble/Editar/${item.idInmueble}" 
                                       class="btn btn-sm btn-primary" title="Editar">
                                        <i class="fa-solid fa-pen-to-square"></i>
                                    </a>
                                    <button type="button" 
                                            class="btn btn-sm btn-danger btn-eliminar" 
                                            data-id="${item.idInmueble}" title="Eliminar">
                                        <i class="fa-solid fa-trash"></i>
                                    </button>
                                </div>
                            </td>
                        </tr>`;
				});
			})
			.catch((err) => console.error("Error al cargar inmuebles:", err));
	}

	//  evento filtro
	selEstado.addEventListener("change", cargarInmuebles);
	selPropietario.addEventListener("change", cargarInmuebles);

	// elimino
	tabla.addEventListener("click", function (e) {
		if (e.target.closest(".btn-eliminar")) {
			const id = e.target.closest(".btn-eliminar").dataset.id;

			Swal.fire({
				title: "¿Seguro?",
				text: "El inmueble quedará dado de baja.",
				icon: "warning",
				showCancelButton: true,
				confirmButtonText: "Sí, eliminar",
				cancelButtonText: "Cancelar"
			}).then(result => {
				if (result.isConfirmed) {
					fetch(`/Inmueble/Borrar`, {
						method: "POST",
						headers: { "Content-Type": "application/x-www-form-urlencoded" },
						body: `id=${id}`
					})
						.then(res => res.json())
						.then(data => {
							if (data.success) {
								Swal.fire("Eliminado", data.mensaje, "success");
								//  refrescamos tabla sin recargar página
								cargarInmuebles();
							} else {
								Swal.fire("Error", data.mensaje, "error");
							}
						});
				}
			});
		}
	});

	// carga inicial
	cargarInmuebles();
});
