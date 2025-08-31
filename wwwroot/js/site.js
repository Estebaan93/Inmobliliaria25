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
});
