// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getTechnicianNameById(technicianId) {
    $.ajax({
        url: "/Technicians/" + technicianId,
        method: "GET",
        success: function (technician) {
            $("#technicianName").text(technician.name);
        }
    });
}