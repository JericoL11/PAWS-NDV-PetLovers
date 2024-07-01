// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Numbers/Decimal only for inputs
function validateDecimalInput(input) {
    input.value = input.value.replace(/[^0-9.]/g, ''); // Allow only numbers and periods
    if ((input.value.match(/\./g) || []).length > 1) {
        input.value = input.value.replace(/\.+$/, ""); // Remove extra periods
    }
}


//auto fill date
$(document).ready(function () {
    // Get the current date
    var currentDate = new Date();

    // Format the date in YYYY-MM-DD for input field
    var formattedDate = currentDate.getFullYear() + '-' +
        ('0' + (currentDate.getMonth() + 1)).slice(-2) + '-' +
        ('0' + currentDate.getDate()).slice(-2);

    // Set the value of the input field with id 'dateInput'
    $('#dateInput, .dateInput').val(formattedDate);
});

//max date for input type date
$(document).ready(function () {
    // Get the current date
    var today = new Date().toISOString().split('T')[0];

    // Set max attribute to today's date
    $('.maxDate').attr('max', today);
});

//CREATE -- alert notification For success duration
$(document).ready(function () {
    var alert = $('#success-alert');
    if (alert.length) {
        setTimeout(function () {
            alert.alert('close');
        }, 5000); // 5 seconds
    }
});


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
            if ($(this).attr('id') !== 'dateInput') {
                $(this).val(''); // Clear the value of the input/select except dateInput
            }
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

    // Function to set the max date for birthdate fields

    function setMaxBirthdate() {
        var today = new Date().toISOString().split('T')[0];
        $('.birthdate').attr('max', today);
    }

    // Auto-fill age field based on birthdate // owner create pets
    $(document).on('change', '.birthdate', function () {
        var birthdate = $(this).val();
        var age = calculateAge(birthdate);
        $(this).closest('tr').find('.age').val(age);
    });


    //pets
    // Auto-fill age field based on birthdate
    $(document).on('change', '.birthdate', function () {
        var birthdate = $(this).val();
        var age = calculateAge(birthdate);
        $(this).closest('.petrow').find('.age').val(age);
    });





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
});
