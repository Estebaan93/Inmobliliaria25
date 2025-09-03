document.addEventListener("DOMContentLoaded", function () {
	//  Confirmación para Inquilinos
	document.querySelectorAll("form.form-borrar-inquilino").forEach(form => {
		form.addEventListener("submit", function (e) {
			e.preventDefault();
			Swal.fire({
				title: "¿Seguro que deseas eliminar este inquilino?",
				text: "El registro quedará dado de baja.",
				icon: "warning",
				showCancelButton: true,
				confirmButtonText: "Sí, eliminar",
				cancelButtonText: "Cancelar"
			}).then((result) => {
				if (result.isConfirmed) {
					form.submit();
				}
			});
		});
	});

	//  Confirmación para Propietarios
	document.querySelectorAll("form.form-borrar-propietario").forEach(form => {
		form.addEventListener("submit", function (e) {
			e.preventDefault();
			Swal.fire({
				title: "¿Seguro que deseas eliminar este propietario?",
				text: "El registro quedará dado de baja.",
				icon: "warning",
				showCancelButton: true,
				confirmButtonText: "Sí, eliminar",
				cancelButtonText: "Cancelar"
			}).then((result) => {
				if (result.isConfirmed) {
					form.submit();
				}
			});
		});
	});

	//  Tipos de Inmuebles
	const btnTipos = document.getElementById("btnTiposInmuebles");
	if (btnTipos) {
		btnTipos.addEventListener("click", function (e) {
			e.preventDefault();
			cargarTipos();
		});
	}

	// Listar Tipos
	function cargarTipos() {
		fetch("/Tipo/ListarJson")
			.then(res => res.json())
			.then(data => {
				let html = `
                    <button id="btnNuevoTipo" class="btn btn-success mb-3">
                        <i class="fa-solid fa-plus"></i> Nuevo Tipo
                    </button>
                    <table class="table table-striped table-hover table-bordered tipos">
                        <thead class="table-dark">
                            <tr>
                                <th>Observación</th>
                                <th>Acciones</th>
                            </tr>
                        </thead>
                        <tbody>
                `;
				data.forEach(item => {
					html += `
                        <tr data-id="${item.idTipo}">
                            <td>${item.observacion}</td>
                            <td>
                                <button class="btn btn-sm btnEditar" 
                                        data-id="${item.idTipo}" 
                                        data-obs="${item.observacion}">
                                    <i class="fa-solid fa-pen-to-square"></i>
                                </button>
                                <button class="btn btn-sm btnEliminar" 
                                        data-id="${item.idTipo}">
                                    <i class="fa-solid fa-trash"></i>
                                </button>
                            </td>
                        </tr>
                    `;
				});
				html += "</tbody></table>";

				Swal.fire({
					title: "Tipos de Inmuebles",
					html: `<div class="table-responsive">${html}</div>`,
					width: 700,
					showConfirmButton: false,
					showCloseButton: true,
					didRender: () => {
						document.getElementById("btnNuevoTipo").addEventListener("click", nuevoTipo);
						document.querySelectorAll(".btnEditar").forEach(b => b.addEventListener("click", editarTipo));
						document.querySelectorAll(".btnEliminar").forEach(b => b.addEventListener("click", eliminarTipo));
					}
				});
			});
	}

	// Crear Tipo
	function nuevoTipo() {
		Swal.fire({
			title: "Nuevo Tipo",
			input: "text",
			inputLabel: "Observación",
			showCancelButton: true,
			confirmButtonText: "Guardar"
		}).then(result => {
			if (result.isConfirmed) {
				fetch("/Tipo/CrearJson", {
					method: "POST",
					headers: { "Content-Type": "application/json" },
					body: JSON.stringify({ observacion: result.value })
				}).then(() => cargarTipos());
			}
		});
	}

	// Editar Tipo
	function editarTipo(e) {
		const id = e.currentTarget.dataset.id;
		const obs = e.currentTarget.dataset.obs;

		Swal.fire({
			title: "Editar Tipo",
			input: "text",
			inputValue: obs,
			showCancelButton: true,
			confirmButtonText: "Guardar"
		}).then(result => {
			if (result.isConfirmed) {
				fetch("/Tipo/EditarJson", {
					method: "POST",
					headers: { "Content-Type": "application/json" },
					body: JSON.stringify({ idTipo: id, observacion: result.value })
				}).then(() => cargarTipos());
			}
		});
	}

	// Eliminar Tipo
	function eliminarTipo(e) {
		const id = e.currentTarget.dataset.id;
		Swal.fire({
			title: "¿Seguro?",
			text: "Se eliminará el tipo",
			icon: "warning",
			showCancelButton: true,
			confirmButtonText: "Sí, eliminar",
			cancelButtonText: "Cancelar"
		}).then(result => {
			if (result.isConfirmed) {
				fetch("/Tipo/EliminarJson", {
					method: "POST",
					headers: { "Content-Type": "application/json" },
					body: JSON.stringify(id)
				}).then(() => cargarTipos());
			}
		});
	}
	document.getElementById("selectInmueble").addEventListener("change", function () {
		var selected = this.options[this.selectedIndex];
		var precio = selected.getAttribute("data-precio") || "";

		document.getElementById("PropietarioNombre").value = selected.getAttribute("data-propietario") || "";
		document.getElementById("PrecioVisible").value = precio;
		document.getElementById("MontoHidden").value = precio; //  AHORA se guarda en Contrato.Monto
	});


});

// Esperar a que cargue el DOM
document.addEventListener("DOMContentLoaded", function () {
	document.querySelectorAll(".btn-editar").forEach(btn => {
		btn.addEventListener("click", function () {
			const idPago = this.dataset.id;
			const detalle = this.dataset.detalle;
			const fecha = this.dataset.fecha;
			const importe = this.dataset.importe;
			const numero = this.dataset.numero;

			Editar(idPago, detalle, fecha, importe, numero);
		});
	});
});

// Función Editar con SweetAlert
function Editar(idPago, detalle, fecha, importe, numero) {
	Swal.fire({
		title: 'Editar Pago',
		html: `
            <div class="text-start">
                <p><b>📅 Fecha:</b> ${fecha}</p>
                <p><b>💰 Importe:</b> $${importe}</p>
                <p><b>#️⃣ Número de Pago:</b> ${numero}</p>
                <label><b>📝 Detalle:</b></label>
                <input id="swal-input-detalle" class="swal2-input" value="${detalle || ''}">
            </div>
        `,
		focusConfirm: false,
		showCancelButton: true,
		confirmButtonText: 'Guardar',
		cancelButtonText: 'Cancelar',
		preConfirm: () => {
			const nuevoDetalle = document.getElementById('swal-input-detalle').value.trim();
			if (!nuevoDetalle) {
				Swal.showValidationMessage('El detalle no puede estar vacío');
			}
			return nuevoDetalle;
		}
	}).then((result) => {
		if (result.isConfirmed) {
			fetch('/Pago/Editar', {
				method: 'POST',
				headers: { 'Content-Type': 'application/json' },
				body: JSON.stringify({ IdPago: idPago, Detalle: result.value })
			})
				.then(response => {
					if (response.ok) {
						Swal.fire('✅ Actualizado')
							.then(() => location.reload());
					} else {
						Swal.fire('❌ Error');
					}
				})
				.catch(() => Swal.fire('❌ Error'));
		}
	});
}
