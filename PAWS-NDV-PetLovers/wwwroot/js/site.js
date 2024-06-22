// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




/*form-add-for-pet*/

$(document).ready(function () {
    // Function to update the name attributes of the rows
    function updateRowNames() {
        $('#PetTable tbody tr').each(function (index) {
            $(this).find('input, select').each(function () {
                var name = $(this).attr('name');
                if (name) {
                    var newName = name.replace(/\d+/, index);
                    $(this).attr('name', newName);
                }
            });
        });
    }

    // Add row
    $('#addRow').click(function () {
        var rowCount = $('#PetTable tbody tr').length;
        var newRow = $('.detailRow').first().clone();

        newRow.find('input, select').each(function () {
            var name = $(this).attr('name');
            if (name) {
                var newName = name.replace(/\d+/, rowCount);
                $(this).attr('name', newName);
            }
            $(this).val(''); // Clear the value of the input/select
        });

        newRow.appendTo($('#PetTable tbody'));
    });

    // Remove row
    $(document).on('click', '.removeRow', function () {
        if ($('#PetTable tbody tr').length > 1) {
            $(this).closest('tr').remove();
            updateRowNames(); // Update the name attributes of the remaining rows
        }
    });

    // =================

<<<<<<< HEAD
    // Auto-fill age field based on birthdate for create
    $(document).on('change', '.birthdate', function () {
        var birthdate = $(this).val();
        var age = calculateAge(birthdate);
        $(this).closest('tr').find('.age').val(age);
    });


    //for pets
    $(document).on('change', '.birthdate', function () {
        var birthdate = $(this).val();
        var age = calculateAge(birthdate);
        $(this).closest('.petrow').find('.age').val(age);
    });


=======
>>>>>>> 298c220e5047a6c7fef5216e0a1f317282d4992f
    // Function to set the max date for birthdate fields
    function setMaxBirthdate() {
        var today = new Date().toISOString().split('T')[0];
        $('.birthdate').attr('max', today);
    }
<<<<<<< HEAD
=======

    // Auto-fill age field based on birthdate
    $(document).on('change', '.birthdate', function () {
        var birthdate = $(this).val();
        var age = calculateAge(birthdate);
        $(this).closest('tr').find('.age').val(age);
    });

>>>>>>> 298c220e5047a6c7fef5216e0a1f317282d4992f
    // Function to calculate age
    function calculateAge(birthdate) {
        var today = new Date();
        var birthDate = new Date(birthdate);
        var age = today.getFullYear() - birthDate.getFullYear();
        var month = today.getMonth() - birthDate.getMonth();
        if (month < 0 || (month === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        return age;
    }

    setMaxBirthdate(); // Set max date on initial load
});
<<<<<<< HEAD




/*

//get the pet id when clicking the modal edit
document.addEventListener('DOMContentLoaded', function () {
    $('#editPetModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var petId = button.data('id');

        var modal = $(this);
        $.get('/Owners/EditPet', { id: petId }, function (data) {
            modal.find('.modal-body').html($(data).find('.modal-body').html());
        });
    });
});*/
=======
>>>>>>> 298c220e5047a6c7fef5216e0a1f317282d4992f
