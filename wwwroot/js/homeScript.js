
//wwwwroot/js/homeScript.js
let btnFiltrar = document.querySelector("#btnFiltrarInmueblesIndex");
let btnLimpiar = document.querySelector("#limpiarFiltros");

btnFiltrar.addEventListener("click", () => {
    let idTipo = document.querySelector("#selectTipos").value;
    let ambientes = document.querySelector("#selectAmbientes").value;
    let cochera = document.querySelector("#selectCochera").value;
    let ordenPrecio = document.querySelector("#selectOrdenPrecio").value;

    axios.get("/Home/GetInmueblesFiltrados", {
        params: {
            idTipo: idTipo != "0" ? idTipo : null,
            ambientes: ambientes != "0" ? ambientes : null,
            cochera: cochera !== "" ? cochera : null,
            ordenPrecio: ordenPrecio || null
        }
    })
    .then(res => llenarCartasInmuebles(res.data))
    .catch(err => console.log(err));
});

btnLimpiar.addEventListener("click", () => {
    axios.get("/Home/GetTodos")
        .then(res => llenarCartasInmuebles(res.data))
        .catch(err => console.log(err));
});

const llenarCartasInmuebles = (inmuebles) => {
    let container = document.querySelector(".containerInmueble");
    container.innerHTML = "";
    let maqueta = "";
    for(let i of inmuebles){
        maqueta += `
        <div class="col-md-4 mb-3">
            <a href="/Inmueble/Detalle/${i.idInmueble}" class="text-decoration-none">
                <div class="card h-100">
                    <img src="${i.urlImagen || '/img/no-image.jpg'}" class="card-img-top" style="height:200px; object-fit:cover;" />
                    <div class="card-body">
                        <h5>${i.tipo.observacion}</h5>
                        <p>${i.descripcion}</p>
                        <p><strong>Precio:</strong> $${i.precio}</p>
                        <p><strong>Ambientes:</strong> ${i.cantidadAmbientes}</p>
                        <p><strong>Cochera:</strong> ${i.cochera ? "Sí" : "No"}</p>
                    </div>
                </div>
            </a>
        </div>`;
    }
    container.innerHTML = maqueta;
};
