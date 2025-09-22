// wwwroot/js/homeScript.js - fetch-only, carga todos y filtra en cliente

console.log("homeScript.js (fetch) loaded:", location.href);

document.addEventListener("DOMContentLoaded", function () {

  const selTipo = document.querySelector("#selectTipos");
  const selAmbientes = document.querySelector("#selectAmbientes");
  const selCochera = document.querySelector("#selectCochera");
  const selOrden = document.querySelector("#selectOrdenPrecio");
  const btnFiltrar = document.querySelector("#btnFiltrarInmueblesIndex");
  const btnLimpiar = document.querySelector("#limpiarFiltros");
  const container = document.querySelector(".containerInmueble");
  const selContratoDias = document.querySelector("#selectContratoDias");

  let cacheInmuebles = []; // cache de todos los inmuebles recibidos

  function parseFilters() {
    const idTipoRaw = selTipo?.value;
    const ambientesRaw = selAmbientes?.value;
    const cocheraRaw = selCochera?.value;
    const ordenRaw = selOrden?.value;
    const contratoRow = selContratoDias?.value;

    return {
      idTipo: idTipoRaw && idTipoRaw !== "0" ? parseInt(idTipoRaw, 10) : null,
      ambientes: ambientesRaw && ambientesRaw !== "0" ? parseInt(ambientesRaw, 10) : null,
      contratoDias: contratoRow && contratoRow !=="" ? parseInt(contratoRow, 10) : null,
      cochera: cocheraRaw === "true" ? true : (cocheraRaw === "false" ? false : null),
      ordenPrecio: ordenRaw || null
    };
  }

  function qsFromObj(obj) {
    return Object.entries(obj)
      .filter(([k,v]) => v !== null && v !== undefined && v !== "")
      .map(([k,v]) => `${encodeURIComponent(k)}=${encodeURIComponent(v)}`)
      .join("&");
  }

  async function fetchJson(url, params = {}) {
    const q = qsFromObj(params);
    const full = q ? `${url}?${q}` : url;
    console.log("fetch ->", full);
    const res = await fetch(full, { headers: { "Accept": "application/json" } });
    if (!res.ok) {
      const txt = await res.text().catch(() => "");
      throw new Error(`HTTP ${res.status} ${res.statusText} - ${txt}`);
    }
    return res.json();
  }

  function aplicarFiltroLocal(filters) {
    let resultado = cacheInmuebles.slice();

    if (filters.idTipo !== null) {
      resultado = resultado.filter(i => (i.tipo?.IdTipo ?? i.tipo?.idTipo ?? i.IdTipo ?? i.idTipo) == filters.idTipo);
    }
    if (filters.ambientes !== null) {
      resultado = resultado.filter(i => (i.cantidadAmbientes ?? i.CantidadAmbientes ?? 0) == filters.ambientes);
    }
    if (filters.cochera !== null) {
      resultado = resultado.filter(i => !!(i.cochera ?? i.Cochera) === filters.cochera);
    }

        // filtro por dias de contrato (intenta diversos nombres de propiedad)
    if (filters.contratoDias !== null) {
      resultado = resultado.filter(i => {
        const dias =
          i.contratoDias ??
          i.DiasContrato ??
          i.diasContrato ??
          i.diasRestantes ??
          i.diasRestantesContrato ??
          i.diasParaFinContrato ??
          i.diasParaVencimiento ??
          null;
        if (dias == null) return false; // si no hay info de contratos, ocultar por seguridad
        return Number(dias) === Number(filters.contratoDias);
      });
    }

    if (filters.ordenPrecio === "asc") {
      resultado.sort((a,b) => (Number(a.precio ?? a.Precio ?? 0) - Number(b.precio ?? b.Precio ?? 0)));
    } else if (filters.ordenPrecio === "desc") {
      resultado.sort((a,b) => (Number(b.precio ?? b.Precio ?? 0) - Number(a.precio ?? a.Precio ?? 0)));
    }

    return resultado;
  }

  function llenarCartasInmuebles(inmuebles) {
    if (!container) {
      console.warn("containerInmueble no encontrado.");
      return;
    }

    if (!Array.isArray(inmuebles) || inmuebles.length === 0) {
      container.innerHTML = `<div class="col-12 text-center py-4">No se encontraron inmuebles.</div>`;
      return;
    }

    let html = "";
    for (const i of inmuebles) {
      const id = i.idInmueble ?? i.IdInmueble ?? "";
      const urlImg = i.urlImagen ?? i.UrlImagen ?? "/img/no-image.jpg";
      const tipo = i.tipo?.observacion ?? i.tipo?.Observacion ?? i.tipo?.Observacion ?? "";
      const descripcion = i.descripcion ?? i.Descripcion ?? "";
      const precio = i.precio ?? i.Precio ?? "";
      const ambientes = i.cantidadAmbientes ?? i.CantidadAmbientes ?? "";
      const cochera = (i.cochera ?? i.Cochera) ? "Sí" : "No";

      html += `
        <div class="col-md-4 mb-3">
          <a href="/Inmueble/Detalle/${id}" class="text-decoration-none">
            <div class="card h-100 shadow-sm">
              <img src="${urlImg}" class="card-img-top" style="height:200px; object-fit:cover;" />
              <div class="card-body">
                <h5 class="card-title">${tipo}</h5>
                <p class="card-text">${descripcion}</p>
                <p><strong>Precio:</strong> $${precio}</p>
                <p><strong>Ambientes:</strong> ${ambientes}</p>
                <p><strong>Cochera:</strong> ${cochera}</p>
              </div>
            </div>
          </a>
        </div>`;
    }
    container.innerHTML = html;
  }

  window.llenarCartasInmuebles = llenarCartasInmuebles;

  async function cargarTodos() {
    try {
      cacheInmuebles = await fetchJson("/Inmueble/ListarDisponible");
      console.log("cargarTodos -> recibidos:", cacheInmuebles.length);
      console.log("cacheInmuebles:", cacheInmuebles);
      llenarCartasInmuebles(cacheInmuebles);
    } catch (err) {
      console.error("Error cargarTodos:", err);
    }
  }

  async function aplicarFiltro() {
    try {
      const filters = parseFilters();
      console.log("aplicarFiltro params:", filters);
      // Si no tenemos cache, cargar todos primero
      if (!cacheInmuebles || cacheInmuebles.length === 0) {
        await cargarTodos();
      }
      const res = aplicarFiltroLocal(filters);
      console.log("aplicarFiltro -> resultados:", res.length);
      llenarCartasInmuebles(res);
    } catch (err) {
      console.error("Error aplicando filtro:", err);
    }
  }

  // listeners
  if (btnFiltrar) btnFiltrar.addEventListener("click", aplicarFiltro);
  else [selTipo, selAmbientes, selCochera, selContratoDias,selOrden].forEach(s => s?.addEventListener("change", aplicarFiltro));

  if (btnLimpiar) {
    btnLimpiar.addEventListener("click", async () => {
      if (selTipo) selTipo.value = "0";
      if (selAmbientes) selAmbientes.value = "0";
      if (selCochera) selCochera.value = "";
      if(selContratoDias) selContratoDias.value = "";
      if (selOrden) selOrden.value = "";
      await cargarTodos();
    });
  }

  // carga inicial cuando el contenedor está vacío
  if (container && container.children.length === 0) {
    cargarTodos();
  }

});