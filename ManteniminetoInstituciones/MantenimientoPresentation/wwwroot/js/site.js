// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function filtrarOpcionesSelect(input, selectId, resultadosId) {
    const select = document.getElementById(selectId);
    const resultados = document.getElementById(resultadosId);
    if (!select || !resultados) return;

    const normalizar = texto => texto
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '')
        .toLocaleLowerCase();
    const termino = normalizar(input.value.trim());
    resultados.replaceChildren();

    if (!termino) {
        resultados.classList.add('d-none');
        return;
    }

    const coincidencias = Array.from(select.options).filter(option =>
        option.value && normalizar(option.text).includes(termino)
    );

    if (coincidencias.length === 0) {
        const vacio = document.createElement('div');
        vacio.className = 'list-group-item text-muted small';
        vacio.textContent = 'No se encontraron coincidencias.';
        resultados.appendChild(vacio);
    } else {
        coincidencias.slice(0, 50).forEach(option => {
            const resultado = document.createElement('button');
            resultado.type = 'button';
            resultado.className = 'list-group-item list-group-item-action text-start';
            resultado.textContent = option.text;
            resultado.addEventListener('click', () => {
                select.value = option.value;
                input.value = option.text;
                resultados.classList.add('d-none');
                select.dispatchEvent(new Event('change', { bubbles: true }));
            });
            resultados.appendChild(resultado);
        });

        if (coincidencias.length > 50) {
            const aviso = document.createElement('div');
            aviso.className = 'list-group-item text-muted small';
            aviso.textContent = `Se muestran 50 de ${coincidencias.length} coincidencias. Escriba más para acotar la búsqueda.`;
            resultados.appendChild(aviso);
        }
    }

    resultados.classList.remove('d-none');
}

function limpiarBusquedaSelect(inputId, selectId, resultadosId) {
    const input = document.getElementById(inputId);
    if (!input) return;

    input.value = '';
    filtrarOpcionesSelect(input, selectId, resultadosId);
    input.focus();
}

function limpiarBuscadorAlSeleccionar(selectId, inputId, resultadosId) {
    const select = document.getElementById(selectId);
    if (!select) return;

    select.addEventListener('change', () => {
        const input = document.getElementById(inputId);
        const resultados = document.getElementById(resultadosId);

        if (input) input.value = '';
        if (resultados) resultados.classList.add('d-none');
    });
}

document.addEventListener('DOMContentLoaded', () => {
    limpiarBuscadorAlSeleccionar('selectDafFiltro', 'txtBuscarDafFiltro', 'resultadosBusquedaDaf');
    limpiarBuscadorAlSeleccionar('selectCapituloFiltro', 'txtBuscarCapituloFiltro', 'resultadosBusquedaCapituloFiltro');
    limpiarBuscadorAlSeleccionar('selectCapitulo', 'txtBuscarCapitulo', 'resultadosBusquedaCapitulo');
    limpiarBuscadorAlSeleccionar('selectInstitucion', 'txtBuscarInstitucion', 'resultadosBusquedaInstitucion');
});
