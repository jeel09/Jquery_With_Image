$(document).ready(function () {
    showCity();
    $('#country').attr('disable', true);
    LoadCountries();
    $('#state').attr('disable', true);
    
})

function showCity() {
    $.ajax({
        url: 'CityManager/CityList',
        type: 'GET',
        dataT: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result, statu, xhr) {
            var obj = "";
            $.each(result, function (index, item) {
                obj += '<tr>';
                obj += '<td>' + item.cityId + '</td>';
                obj += '<td>' + item.cityName + '</td>';
                obj += '<td>' + item.country.countryName + '</td>';
                obj += '<td>' + item.state.stateName + '</td>';
                obj += '<td><a href="#" class="btn btn-primary" onclick="Edit(' + item.cityId + ')">Edit</a> ||  <a href="#" class="btn btn-danger" onclick="Delete(' + item.cityId + ')">Delete</a></td >';
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
        url: '/CityManager/GetCountry',
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
        url: '/CityManager/GetState?countryId=' + countryId,
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
    });
}
$('#btnAdd').click(function () {
    ClearTextBox();
    $('span').remove();
    LoadCountries();
    LoadStates();
    $('#userModal').modal('show');
    $('#cId').hide();
    $('#AddData').show();
    $('#UpdateData').hide();
    $('#addCity').text('Add City');
});
function giveErrorCity() {
    var cityName = $('#CityName').val();
    var country = $('#country').val();
    var state = $('#state').val();
    if (!cityName) {
        if ($('#CityNameError').length === 0) {
            $('#CityName').keyup(function () {
                if ($(this).val()) {
                    $('#CityNameError').remove();
                }
            });
            $('#CityName').after('<span id="CityNameError" class="text-danger">City Name is required.</span>');
        }
    }
    if (!country) {
        if($('#CountryNameError').length === 0){
            $('#country').change(function () {
                if ($(this).val()) {
                    $('#CountryNameError').remove();
                }
            });
            $('#country').after('<span id="CountryNameError" class="text-danger">Country is required.</span>');
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
        return;
    }
}
function AddCity() {
    giveErrorCity();
    var objData = {
        CityName: $('#CityName').val(),
        CountryId: $('#country').val(),
        StateId: $('#state').val()
    }
    $.ajax({
        url: '/CityManager/AddCity',
        type: 'POST',
        data: objData,
        contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                LoadCountries();
                LoadStates();
                alert('City Created Successfully');
                ClearTextBox();
                showCity();
                HidePop();
            }
            else {
                alert('City not Created Successfully' + response.message);
            }
        },
        error: function () {
            alert("City not Created Successfully");
        }
    });
};

function HidePop() {
    $('#userModal').modal('hide');
}

function ClearTextBox() {
    $('#CityName').val('');
}

function Delete(cityId) {
    if (confirm('Are You Sure, You want to delete the City? ')) {
        $.ajax({
            url: '/CityManager/Delete?id=' + cityId,
            success: function (response) {
                if (response.success) {
                    alert('City Deleted SuccessFully!');
                    showCity();
                }
                else {
                    alert('City not Deleted: ' + response.message);
                }
            },
            error: function () {
                alert("City not Deleted");
            }
        });
    }
}

function Edit(cityId) {
    $('span').remove();
    $.ajax({
        url: '/CityManager/Edit?cityId=' + cityId,
        type: 'GET',
        contentType: 'application/x-www-form-urlencoded;charset=utf-8',
        dataType: 'json',
        success: function (response) {
            $('#userModal').modal('show');
            $('#CityId').val(response.cityId);
            $('#CityName').val(response.cityName);
            $('#country').val(response.countryId);
            $('#state').val(response.stateId);
            LoadStates(response.countryId, response.stateId);

            $('#AddData').hide();
            $('#UpdateData').show();
        },
        error: function () {
            alert("City not Found");
        }
    });
}

function UpdateCity() {
    giveErrorCity();
    var objData = {
        CityId: $('#CityId').val(),
        CityName: $('#CityName').val(),
        CountryId: $('#country').val(),
        StateId: $('#state').val()
    }
    $.ajax({
        url: '/CityManager/EditCity',
        type: 'POST',
        data: objData,
        contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                alert('City Updated Successfully');
                ClearTextBox();
                showCity();
                HidePop();
            }
            else {
                alert('City can not Updated Successfully' + response.message);
            }
        },
        error: function () {
            alert("City can't Updated");
        }
    });

    function HidePop() {
        $('#userModal').modal('hide');
    }

    function ClearTextBox() {
        $('#CityName').val('');
    }
}