$(document).ready(function () {
    showUser();
    LoadGenders();
    LoadHobbies();
    $('#country').attr('disable', true);
    LoadCountries();
    $('#state').attr('disable', true);
    $('#city').attr('disable', true);
})
function showUser() {
    $.ajax({
        url: 'UserManager/UserList',
        type: 'GET',
        dataT: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result, statu, xhr) {
            var obj = "";
            $.each(result, function (index, item) {
                obj += '<tr>';
                obj += '<td>' + item.userId + '</td>';
                obj += '<td>' + item.firstName + '</td>';
                obj += '<td>' + item.lastName + '</td>';
                obj += '<td>' + item.address + '</td>';
                obj += '<td>' + item.gender.genderName + '</td>';
                obj += '<td>' + item.hobbyId + '</td>';
                obj += '<td>' + item.country.countryName + '</td>';
                obj += '<td>' + item.state.stateName + '</td>';
                obj += '<td>' + item.city.cityName + '</td>';
                obj += '<td><img src="' + item.imageUrl + '" alt="User Image" height="100" width="100" class="rounded"></td>';

                obj += '<td><a href="#" class="btn btn-primary" onclick="Edit(' + item.userId + ')">Edit</a> ||  <a href="#" class="btn btn-danger" onclick="Delete(' + item.userId + ')">Delete</a></td >';
                obj += '</tr>';
            })
            $('#tblData').html(obj);
        },
        error: function () {
            alert("Data can't get");
        }
    });
};
function LoadCountries() {
    $('#country').empty();

    $.ajax({
        url: '/UserManager/GetCountry',
        success: function (response) {

            if (response != null && response != undefined && response.length > 0) {
                $('#country').attr('disable', true);
                $('#country').append('<option disabled selected>--- Select Country ---</option>');

                $.each(response, function (i, data) {

                    $('#country').append('<option value= ' + data.countryId + '>' + data.countryName + '</option>');
                });
            }
            else {
                $('#country').attr('disable', true);
                $('#country').append('<option disabled selected>---  Countries not Available ---</option>');
            }
        }
    }).done(function () {
        $('#country').on('change', function () {
            var selectedCountryId = $(this).val();
            LoadStates(selectedCountryId);
        });
    });
}
function LoadStates(countryId, selectedStateId) {
    $('#state').empty();

    $.ajax({
        url: '/UserManager/GetState?countryId=' + countryId,
        success: function (response) {

            if (response != null && response != undefined && response.length > 0) {
                $('#state').attr('disable', true);
                $('#state').find('option').remove();
                $('#state').append('<option disabled selected>--- Select State ---</option>');

                $.each(response, function (i, data) {
            
                    $('#state').append('<option value= ' + data.stateId + '>' + data.stateName + '</option>');
                });
                $('#state').val(selectedStateId);
            }
            else {
                $('#state').attr('disable', true);
                $('#state').append('<option disabled selected>---  States not Available ---</option>');
            }
        }
    }).done(function () {
        // Add event listener to state dropdown
        $('#state').on('change', function () {
            var selectedStateId = $(this).val();
            LoadCities(selectedStateId);
        });
    });
}
function LoadCities(stateId, selectedCityId) {
    $('#city').empty();

    $.ajax({
        url: '/UserManager/GetCity?stateId=' + stateId,
        success: function (response) {

            if (response != null && response != undefined && response.length > 0) {
                $('#city').attr('disable', true);
                $('#city').find('option').remove();
                $('#city').append('<option disabled selected>--- Select City ---</option>');

                $.each(response, function (i, data) {

                    $('#city').append('<option value= ' + data.cityId + '>' + data.cityName + '</option>');
                });
                $('#city').val(selectedCityId);
            }
            else {
                $('#city').attr('disable', true);
                $('#city').append('<option disabled selected>---  Cities not Available ---</option>');
            }
        }
    });
}
function LoadGenders() {
    $('#gender').empty();

    $.ajax({
        url: '/UserManager/GetGender',
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                //$('#gender').attr('disable', true);
                $.each(response, function (i, data) {
                    $('#gender').append('<input type="radio" name="gender" value="' + data.genderId + '">' + data.genderName + '<br>');
                });
            }
            else {
                //$('#gender').attr('disable', true);
                $('#gender').append('<input type="radio" name="gender" value="" disabled>No Genders Available</input>');
            }
        }
    });
}

function LoadHobbies() {
    $('#hobby').empty();

    $.ajax({
        url: '/UserManager/GetHobby',
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                $.each(response, function (i, data) {
                    $('#hobby').append('<input type="checkbox" class="hobby" value="' + data.hobbyName + '">' + data.hobbyName + '<br>');
                });
            }
            else {
                $('#hobby').append('<input type="checkbox" class="hobby" value="" disabled>No Hobbies Available</input>');
            }
        }
    });
}

$('#btnAdd').click(function () {
    ClearTextBox();
    $('#userImage').empty();
    $('span').remove();
    LoadCountries();
    LoadStates();
    LoadCities();
    $('#userModal').modal('show');
    $('#cId').hide();
    $('#AddData').show();
    $('#UpdateData').hide();
    $('#addUser').text('Add User');
});

function giveErrorUser() {
    var firstName = $('#FirstName').val();
    var lastName = $('#LastName').val();
    var address = $('#Address').val();
    var GenderId = $('input[name="gender"]:checked').val();
    var HobbyId = $('.hobby:checked').val();
    var country = $('#country').val();
    var state = $('#state').val();
    var city = $('#city').val();

    if (!firstName) {
        if ($('#FirstNameError').length === 0) {
            $('#FirstName').keyup(function () {
                if ($(this).val()) {
                    $('#FirstNameError').remove();
                }
            });
            $('#FirstName').after('<span id="FirstNameError" class="text-danger">First Name is required.</span>');
        }
    }
    if (!lastName) {
        if ($('#LastNameError').length === 0) {
            $('#LastName').keyup(function () {
                if ($(this).val()) {
                    $('#LastNameError').remove();
                }
            });
            $('#LastName').after('<span id="LastNameError" class="text-danger">Last Name is required.</span>');
        }
    }
    if (!address) {
        if ($('#AddressError').length === 0) {
            $('#Address').keyup(function () {
                if ($(this).val()) {
                    $('#AddressError').remove();
                }
            });
            $('#Address').after('<span id="AddressError" class="text-danger">Address is required.</span>');
        }
    }
    if (!GenderId) {
        if ($('#GenderError').length === 0) {
            $('input[name="gender"]').change(function () {
                if ($(this).val()) {
                    $('#GenderError').remove();
                }
            });
            $('#gender').after('<span id="GenderError" class="text-danger">Gender is required.</span>');
        }
    }
    if (!HobbyId) {
        if ($('#HobbiesError').length === 0) {
            $('.hobby').change(function () {
                if ($(this).val()) {
                    $('#HobbiesError').remove();
                }
            });
            $('#hobby').after('<span id="HobbiesError" class="text-danger">Hobbies is required.</span>');
        }
    }
    if (!country) {
        if ($('#CountryError').length === 0) {
            $('#country').change(function () {
                if ($(this).val()) {
                    $('#CountryError').remove();
                }
            });
            $('#country').after('<span id="CountryError" class="text-danger">Country is required.</span>');
        }
    }
    if (!state) {
        if ($('#StateError').length === 0) {
            $('#state').change(function () {
                if ($(this).val()) {
                    $('#StateError').remove();
                }
            });
            $('#state').after('<span id="StateError" class="text-danger">State is required.</span>');
        }
    }
    if (!city) {
        if ($('#CityError').length === 0) {
            $('#city').change(function () {
                if ($(this).val()) {
                    $('#CityError').remove();
                }
            });
            $('#city').after('<span id="CityError" class="text-danger">City is required.</span>');
        }
        return;
    }
}

function AddUser() {
    giveErrorUser();

    var selectedHobbies = [];
    $('.hobby:checked').each(function () {
        selectedHobbies.push($(this).val());
    });
    var hobbyChecked = selectedHobbies.join(', ');

    var formData = new FormData();
    formData.append('FirstName', $('#FirstName').val());
    formData.append('LastName', $('#LastName').val());
    formData.append('Address', $('#Address').val());
    formData.append('GenderId', $('input[name="gender"]:checked').val());
    formData.append('HobbyId', hobbyChecked);
    formData.append('CountryId', $('#country').val());
    formData.append('StateId', $('#state').val());
    formData.append('CityId', $('#city').val());
    formData.append('ImageUrl', $('#preview').attr('src'));
    formData.append('file', $('#Image')[0].files[0]);

    $.ajax({
        url: '/UserManager/AddUser',
        type: 'POST',
        data: formData,
        contentType: false,
        cache: false, 
        processData: false, 
        success: function (response) {
            if (response.success) {
                LoadGenders();
                LoadHobbies();
                LoadCountries();
                LoadStates();
                LoadCities();
                alert('User Created Successfully');
                ClearTextBox();
                showUser();
                HidePop();
            }
            else {
                alert('User not Created Successfully' + response.message);
            }
        },
        error: function () {
            alert("User not Created Successfully");
        }
    });
};

function HidePop() {
    $('#userModal').modal('hide');
}

function ClearTextBox() {
    $('#FirstName').val('');
    $('#LastName').val('');
    $('#Address').val('');
    $('input[name="gender"]').prop('checked', false);
    $('.hobby').prop('checked', false);
    $('#image').val('');
    $('#userImage').empty();
}

function Delete(userId) {
    if (confirm('Are You Sure, You want to delete the User? ')) {
        $.ajax({
            url: '/UserManager/Delete?id=' + userId,
            success: function (response) {
                if (response.success) {
                    alert('User Deleted SuccessFully!');
                    showUser();
                }
                else {
                    alert('User not Deleted');
                }
            },
            error: function () {
                alert("User not Deleted");
            }
        });
    }
}

function Edit(userId) {
    $('span').remove();
    $.ajax({
        url: '/UserManager/Edit?userId=' + userId,
        type: 'GET',
        contentType: 'application/x-www-form-urlencoded;charset=utf-8',
        dataType: 'json',
        success: function (response) {
            $('#userModal').modal('show');
            $('#UserId').val(response.userId);
            $('#FirstName').val(response.firstName);
            $('#LastName').val(response.lastName);
            $('#Address').val(response.address);
            $('input[name="gender"][value="' + response.genderId + '"]').prop('checked', true);

            var hobbies = response.hobbyId.split(', ');
            $('.hobby').each(function () {
                if (hobbies.includes($(this).val())) {
                    $(this).prop('checked', true);
                }
                else {
                    $(this).prop('checked', false);
                }
            });

            $('#country').val(response.countryId);
            $('#state').val(response.stateId);
            LoadStates(response.countryId, response.stateId);
            $('#city').val(response.cityId);
            LoadCities(response.stateId, response.cityId);
            $('#preview').attr('src', response.imageUrl);

            var imageUrl = response.imageUrl;
            if (imageUrl) {
                var imgElement = $('<img id="preview" class="rounded">').attr('src', imageUrl).attr('alt', 'User Image').attr('height', '100').attr('width', '100');
                $('#userImage').empty().append(imgElement);
            } else {
                $('#userImage').empty(); // Clear the image element if no image URL is available
            }
            $('#Image').on('change', function () {
                var file = $(this)[0].files[0];
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#preview').attr('src', e.target.result);
                }

                reader.readAsDataURL(file);
            });

            $('#AddData').hide();
            $('#UpdateData').show();
        },
        error: function () {
            alert("User not Found");
        }
    });
}

function UpdateUser() {
    giveErrorUser();

    var selectedHobbies = [];
    $('.hobby:checked').each(function () {
        selectedHobbies.push($(this).val());
    });
    var hobbyChecked = selectedHobbies.join(', ');

    var formData = new FormData();
    formData.append('UserId', $('#UserId').val());
    formData.append('FirstName', $('#FirstName').val());
    formData.append('LastName', $('#LastName').val());
    formData.append('Address', $('#Address').val());
    formData.append('GenderId', $('input[name="gender"]:checked').val());
    formData.append('HobbyId', hobbyChecked);
    formData.append('CountryId', $('#country').val());
    formData.append('StateId', $('#state').val());
    formData.append('CityId', $('#city').val());
    formData.append('ImageUrl', $('#preview').attr('src'));
    formData.append('file', $('#Image')[0].files[0]); 

    $.ajax({
        url: '/UserManager/EditUser',
        type: 'POST',
        data: formData,
        contentType: false, 
        cache: false, 
        processData: false,
        success: function (response) {
            if (response.success) {
                alert('User Updated Successfully');
                ClearTextBox();
                showUser();
                HidePop();
            }
            else {
                alert('User not Updated' + response.message);
            }
        },
        error: function () {
            alert("User can't Updated");
        }
    });

    function HidePop() {
        $('#userModal').modal('hide');
    }

    function ClearTextBox() {
        $('#FirstName').val('');
        $('#LastName').val('');
        $('#Address').val('');
        $('input[name="gender"]').prop('checked', false);
        $('.hobby').prop('checked', false);
        $('#image').val('');
        $('#userImage').empty();
    }
}