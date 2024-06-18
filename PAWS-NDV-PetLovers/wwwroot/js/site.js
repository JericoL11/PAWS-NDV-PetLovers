// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/*form-add-for-pet*/
/*$(document).ready(function () {
    var petIndex = 1;

    // Add pet input dynamically
    $('#addPet').click(function () {
        var newPetRow = `
                    <div class="row pet-row">
                        <div class="form-group col-md-4 col-sm-12">
                            <label>Pet Name</label>
                            <input name="Pets[${petIndex}].petName" class="form-control bg-light" />
                        </div>

                        <div class="form-group col-md-4 col-sm-12">
                            <label>Species</label>
                            <select name="Pets[${petIndex}].species" class="form-control bg-light">
                                <option value="" selected hidden>Select Species</option>
                                <option value="Aspin">Aspin</option>
                                <option value="Shitzu">Shitzu</option>
                                <option value="Japanese Spitz">Japanese Spitz</option>
                            </select>
                        </div>

                        <div class="form-group col-md-4 col-sm-12">
                            <label>Breed</label>
                            <select name="Pets[${petIndex}].breed" class="form-control bg-light">
                                <option value="" selected hidden>Select Breed</option>
                                <option value="Siamese">Siamese</option>
                                <option value="Ragdoll">Ragdoll</option>
                                <option value="Ragamuffin">Ragamuffin</option>
                            </select>
                        </div>

                        <div class="form-group col-md-4 col-sm-12">
                            <label>Birth Date</label>
                            <input name="Pets[${petIndex}].bdate" type="date" class="form-control bg-light" />
                        </div>

                        <div class="form-group col-md-4 col-sm-12">
                            <label>Gender</label>
                            <select name="Pets[${petIndex}].gender" class="form-control bg-light">
                                <option value="" selected hidden>Select Gender</option>
                                <option value="Male">Male</option>
                                <option value="Female">Female</option>
                            </select>
                        </div>

                        <div class="form-group col-md-4 col-sm-12">
                            <label>Color</label>
                            <input name="Pets[${petIndex}].color" class="form-control bg-light" />
                        </div>

                        <div class="form-group col-md-2 col-sm-12">
                            <button type="button" class="btn btn-danger removePet">Remove</button>
                        </div>
                    </div>
                `;

        $('#PetContainer').append(newPetRow);
        petIndex++;
    });

    // Remove pet input dynamically
    $('#PetContainer').on('click', '.removePet', function () {
        $(this).closest('.pet-row').remove();
        petIndex--; // Decrement index when removing a pet row
    });
});*/