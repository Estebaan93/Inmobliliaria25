document.addEventListener("DOMContentLoaded", function () {
  // Inquilinos
  document.querySelectorAll("form.form-borrar-inquilino").forEach(form => {
    form.addEventListener("submit", function (e) {
      e.preventDefault();
      Swal.fire({
        title: "¿Seguro que deseas eliminar este inquilino?",
        text: "El registro quedará dado de baja.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar"
      }).then((result) => {
        if (result.isConfirmed) {
          form.submit();
        }
      });
    });
  });

  // Propietarios
  document.querySelectorAll("form.form-borrar-propietario").forEach(form => {
    form.addEventListener("submit", function (e) {
      e.preventDefault();
      Swal.fire({
        title: "¿Seguro que deseas eliminar este propietario?",
        text: "El registro quedará dado de baja.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar"
      }).then((result) => {
        if (result.isConfirmed) {
          form.submit();
        }
      });
    });
  });
});