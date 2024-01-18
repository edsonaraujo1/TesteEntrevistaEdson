// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const input = document.getElementById("ValorVeiculo");

input.addEventListener("keyup", formatarMoeda);

function formatarMoeda(e) {

    var v = e.target.value.replace(/\D/g, "");

    v = (v / 100).toFixed(2) + "";

    v = v.replace(".", ",");

    v = v.replace(/(\d)(\d{3})(\d{3}),/g, "$1.$2.$3,");

    v = v.replace(/(\d)(\d{3}),/g, "$1.$2,");

    e.target.value = v;

}