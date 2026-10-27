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

function mostrarNotificacion(mensaje, tipo) {
    const region = document.getElementById('notificationRegion');
    if (!region || !window.bootstrap) return;

    const estilos = {
        success: { color: 'success', icon: 'bi-check-circle-fill' },
        info: { color: 'primary', icon: 'bi-info-circle-fill' }
    };
    const estilo = estilos[tipo] || estilos.info;
    const toast = document.createElement('div');
    toast.className = `toast align-items-center border-0 text-bg-${estilo.color}`;
    toast.setAttribute('role', 'status');
    toast.setAttribute('aria-live', 'polite');
    toast.setAttribute('aria-atomic', 'true');

    const contenido = document.createElement('div');
    contenido.className = 'd-flex';

    const cuerpo = document.createElement('div');
    cuerpo.className = 'toast-body d-flex align-items-start gap-2';

    const icono = document.createElement('i');
    icono.className = `bi ${estilo.icon} mt-1`;
    icono.setAttribute('aria-hidden', 'true');

    const texto = document.createElement('span');
    texto.textContent = String(mensaje || (tipo === 'success' ? 'Los cambios se guardaron correctamente.' : 'Hay información disponible.'));
    cuerpo.append(icono, texto);

    const cerrar = document.createElement('button');
    cerrar.type = 'button';
    cerrar.className = 'btn-close btn-close-white me-2 m-auto';
    cerrar.setAttribute('data-bs-dismiss', 'toast');
    cerrar.setAttribute('aria-label', 'Cerrar aviso');

    contenido.append(cuerpo, cerrar);
    toast.appendChild(contenido);
    region.appendChild(toast);

    toast.addEventListener('hidden.bs.toast', () => toast.remove(), { once: true });
    bootstrap.Toast.getOrCreateInstance(toast, { delay: tipo === 'success' ? 4500 : 6000 }).show();
}

function mostrarBanner(mensaje, tipo) {
    const modalActivo = document.querySelector('.modal.show');
    const region = modalActivo
        ? modalActivo.querySelector('.modal-body')
        : document.getElementById('messageBannerRegion');
    if (!region) return;

    const configuracion = {
        warning: { clase: 'alert-warning', icono: 'bi-exclamation-triangle-fill', titulo: 'Revisa esta información' },
        danger: { clase: 'alert-danger', icono: 'bi-exclamation-circle-fill', titulo: 'No se pudo completar la acción' }
    };
    const estilo = configuracion[tipo] || configuracion.danger;
    const alerta = document.createElement('div');
    alerta.className = `alert ${estilo.clase} alert-dismissible fade show message-banner d-flex gap-3 align-items-start${modalActivo ? ' message-banner--modal' : ''}`;
    alerta.setAttribute('role', tipo === 'warning' ? 'status' : 'alert');

    const icono = document.createElement('i');
    icono.className = `bi ${estilo.icono} message-banner__icon`;
    icono.setAttribute('aria-hidden', 'true');

    const contenido = document.createElement('div');
    contenido.className = 'flex-grow-1';
    const encabezado = document.createElement('strong');
    encabezado.className = 'd-block mb-1';
    encabezado.textContent = estilo.titulo;
    const texto = document.createElement('span');
    texto.textContent = String(mensaje || 'Ocurrió un problema. Intente nuevamente.');
    contenido.append(encabezado, texto);

    const cerrar = document.createElement('button');
    cerrar.type = 'button';
    cerrar.className = 'btn-close';
    cerrar.setAttribute('data-bs-dismiss', 'alert');
    cerrar.setAttribute('aria-label', 'Cerrar aviso');

    alerta.append(icono, contenido, cerrar);
    if (modalActivo) {
        region.querySelectorAll('.message-banner--modal').forEach(aviso => aviso.remove());
        region.prepend(alerta);
        region.scrollTo({ top: 0, behavior: 'smooth' });
    } else {
        region.replaceChildren(alerta);
        alerta.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }
}

function clasificarAviso(mensaje) {
    const texto = String(mensaje || '');
    if (/\b(error|no se pudo|no fue posible|ocurri[oó] un problema|fall[oó])\b/i.test(texto)) {
        return 'danger';
    }
    if (/\b(exitosamente|correctamente|con éxito|se guard[oó] con éxito|se elimin[oó] con éxito)\b/i.test(texto)) {
        return 'success';
    }
    if (/\b(obligatori[oa]s?|requerid[oa]s?|seleccion(e|ar)|complete|ingrese|introduzca|modificaciones sin guardar|se descartarán)\b/i.test(texto)) {
        return 'warning';
    }
    if (/\b(info(?:rmaci[oó]n)?|nota|recuerde|consulte|actualizaci[oó]n disponible)\b/i.test(texto)) {
        return 'info';
    }
    return 'danger';
}

window.alert = mensaje => {
    const texto = String(mensaje || '');
    const tipo = clasificarAviso(texto);
    if (tipo === 'info' || tipo === 'success') {
        mostrarNotificacion(texto, tipo);
        return;
    }
    mostrarBanner(texto, tipo);
};

document.addEventListener('hidden.bs.modal', evento => {
    evento.target.querySelectorAll('.message-banner--modal').forEach(aviso => aviso.remove());
});

function confirmarAccion(mensaje, opciones = {}) {
    const elemento = document.getElementById('confirmationModal');
    const aceptar = document.getElementById('confirmationModalAccept');
    const cancelar = document.getElementById('confirmationModalCancel');
    const texto = document.getElementById('confirmationModalMessage');
    const titulo = document.getElementById('confirmationModalTitle');

    if (!elemento || !aceptar || !texto || !titulo || !window.bootstrap) {
        return Promise.reject(new Error('No se pudo inicializar el diálogo de confirmación.'));
    }

    const modal = bootstrap.Modal.getOrCreateInstance(elemento);
    const destructiva = opciones.kind === 'danger';
    elemento.dataset.kind = destructiva ? 'danger' : opciones.kind === 'warning' ? 'warning' : 'info';
    titulo.textContent = opciones.title || (destructiva ? 'Confirmar eliminación' : 'Confirmar acción');
    texto.textContent = String(mensaje || '¿Desea continuar?');
    aceptar.textContent = opciones.confirmText || (destructiva ? 'Sí, continuar' : 'Continuar');
    cancelar.textContent = opciones.cancelText || 'Cancelar';
    aceptar.className = `btn ${destructiva ? 'btn-danger' : 'btn-cgr-primary'} rounded-pill px-4`;

    return new Promise(resolve => {
        let resuelta = false;
        const finalizar = resultado => {
            if (resuelta) return;
            resuelta = true;
            aceptar.removeEventListener('click', alAceptar);
            elemento.removeEventListener('hidden.bs.modal', alCancelar);
            resolve(resultado);
        };
        const alAceptar = () => {
            finalizar(true);
            modal.hide();
        };
        const alCancelar = () => finalizar(false);

        aceptar.addEventListener('click', alAceptar);
        elemento.addEventListener('hidden.bs.modal', alCancelar, { once: true });
        modal.show();
        aceptar.focus();
    });
}

(() => {
    const fetchOriginal = window.fetch.bind(window);
    const estadoCarga = document.getElementById('requestStatus');
    const botonesEnProceso = new WeakMap();
    let solicitudesPendientes = 0;
    let temporizadorCarga;

    function iniciarCargaBoton(boton) {
        if (!boton || boton.disabled) return null;

        let estado = botonesEnProceso.get(boton);
        if (!estado) {
            estado = { contenido: boton.innerHTML, pendientes: 0 };
            botonesEnProceso.set(boton, estado);
            boton.innerHTML = '<span class="spinner-border spinner-border-sm me-2" aria-hidden="true"></span>Procesando…';
            boton.classList.add('is-loading');
            boton.setAttribute('aria-busy', 'true');
        }

        estado.pendientes++;
        boton.disabled = true;
        return boton;
    }

    function finalizarCargaBoton(boton) {
        if (!boton) return;

        const estado = botonesEnProceso.get(boton);
        if (!estado) return;
        estado.pendientes--;
        if (estado.pendientes > 0) return;

        boton.innerHTML = estado.contenido;
        boton.disabled = false;
        boton.classList.remove('is-loading');
        boton.removeAttribute('aria-busy');
        botonesEnProceso.delete(boton);
    }

    window.fetch = async (...argumentos) => {
        const opciones = argumentos[1] || {};
        const {
            suppressGlobalError = false,
            suppressAutoSuccess = false,
            ...opcionesFetch
        } = opciones;
        const argumentosFetch = argumentos.length > 1 ? [argumentos[0], opcionesFetch] : argumentos;
        const metodo = (opciones.method || (argumentos[0] instanceof Request ? argumentos[0].method : 'GET')).toUpperCase();
        const boton = ['POST', 'PUT', 'PATCH', 'DELETE'].includes(metodo)
            ? iniciarCargaBoton(document.activeElement instanceof HTMLButtonElement ? document.activeElement : null)
            : null;

        solicitudesPendientes++;
        if (solicitudesPendientes === 1) {
            temporizadorCarga = window.setTimeout(() => {
                if (!estadoCarga) return;
                estadoCarga.classList.add('is-active');
                estadoCarga.setAttribute('aria-hidden', 'false');
            }, 180);
        }

        try {
            const respuesta = await fetchOriginal(...argumentosFetch);
            const tipoContenido = respuesta.headers.get('content-type') || '';

            if (!suppressAutoSuccess && respuesta.ok && tipoContenido.includes('application/json')) {
                respuesta.clone().json().then(resultado => {
                    if (resultado && resultado.success === true && typeof resultado.message === 'string') {
                        mostrarNotificacion(resultado.message, 'success');
                    }
                }).catch(error => console.warn('No se pudo interpretar la respuesta de la operación.', error));
            }

            return respuesta;
        } catch (error) {
            if (!suppressGlobalError) {
                mostrarBanner('No se pudo conectar con el servidor. Verifique su conexión e intente nuevamente.', 'danger');
            }
            throw error;
        } finally {
            solicitudesPendientes--;
            finalizarCargaBoton(boton);
            if (solicitudesPendientes === 0) {
                window.clearTimeout(temporizadorCarga);
                if (estadoCarga) {
                    estadoCarga.classList.remove('is-active');
                    estadoCarga.setAttribute('aria-hidden', 'true');
                }
            }
        }
    };
})();

document.addEventListener('DOMContentLoaded', () => {
    limpiarBuscadorAlSeleccionar('selectDafFiltro', 'txtBuscarDafFiltro', 'resultadosBusquedaDaf');
    limpiarBuscadorAlSeleccionar('selectCapituloFiltro', 'txtBuscarCapituloFiltro', 'resultadosBusquedaCapituloFiltro');
    limpiarBuscadorAlSeleccionar('selectCapitulo', 'txtBuscarCapitulo', 'resultadosBusquedaCapitulo');
    limpiarBuscadorAlSeleccionar('selectInstitucion', 'txtBuscarInstitucion', 'resultadosBusquedaInstitucion');

    document.querySelectorAll('form').forEach(formulario => {
        formulario.addEventListener('submit', () => {
            const boton = formulario.querySelector('button[type="submit"], input[type="submit"]');
            if (!boton || boton.disabled) return;

            boton.disabled = true;
            boton.setAttribute('aria-busy', 'true');
            if (boton instanceof HTMLButtonElement) {
                boton.dataset.originalContent = boton.innerHTML;
                boton.innerHTML = '<span class="spinner-border spinner-border-sm me-2" aria-hidden="true"></span>Enviando…';
            } else {
                boton.dataset.originalValue = boton.value;
                boton.value = 'Enviando…';
            }
        });
    });
});
