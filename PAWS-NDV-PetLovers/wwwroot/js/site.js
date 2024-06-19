// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




/*form-add-for-pet*/
$(document).ready(function () {
    //add row
    $('#addRow').click(function () {
        var rowCount = $('#PetTable tbody tr').length;
        var newRow = $('.detailRow').first().clone();

        newRow.find('input').val('');
        newRow.find('input').attr('name', 'Pets[' + rowCount + '].petName');
        newRow.find('input').attr('name', 'Pets[' + rowCount + '].gender');
        newRow.find('input').attr('name', 'Pets[' + rowCount + '].species');
        newRow.find('input').attr('name', 'Pets[' + rowCount + '].breed');
        newRow.find('input').attr('name', 'Pets[' + rowCount + '].bdate');
        newRow.find('input').attr('name', 'Pets[' + rowCount + '].color');
        newRow.appendTo($('#PetTable tbody'));
   
      

    });

    $(document).on('click', '.removeRow', function () {

        if ($('#PetTable tbody tr').length > 1) {
            $(this).closest('tr').remove();
        }
     
    });

});



