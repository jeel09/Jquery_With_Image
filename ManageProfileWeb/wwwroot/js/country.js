$(document).ready(function () {
    showCountry();
});

function showCountry() {
    $.ajax({
        url: 'CountryManager/CountryList',
        type: 'GET',
        dataT: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result, statu, xhr) {
            var obj = "";
            $.each(result, function (index, item) {
                obj += '<tr>';
                obj += '<td>' + item.countryId + '</td>';
                obj += '<td>' + item.countryName + '</td>';
                obj += '<td><a href="#" class="btn btn-primary" onclick="Edit(' + item.countryId + ')">Edit</a> ||  <a href="#" class="btn btn-danger" onclick="Delete(' + item.countryId + ')">Delete</a></td >';
                obj += '</tr>';
            });
            $('#tblData').html(obj);
        },
        error: function () {
            alert("Data can't get");
        }
    });
}

$('#btnAdd').click(function () {
    ClearTextBox();
    $('span').remove();
    $('#userModal').modal('show');
    $('#cId').hide();
    $('#AddData').show();
    $('#UpdateData').hide();
    $('#addCountry').text('Add Country');
});
function giveErrorCountry() {
    var countryName = $('#CountryName').val();
    if (!countryName) { 
        if ($('#CountryNameError').length === 0) { 
            $('#CountryName').keyup(function () {
                if ($(this).val()) { 
                    $('#CountryNameError').remove();
                }
            });
            $('#CountryName').after('<span id="CountryNameError" class="text-danger">Country Name is required.</span>');
        }
        return;
    }
}
function AddCountry() {
    giveErrorCountry();
    var objData = {
        CountryName: $('#CountryName').val()
    };
    $.ajax({
        url: '/CountryManager/AddCountry',
        type: 'POST',
        data: objData,
        contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                alert('Country Created Successfully');
                ClearTextBox();
                showCountry();
                HidePop();
            }
            else {
                alert('Country not Created: ' + response.message);
            }
        },
        error: function () {
            alert("An error occurred while creating the country.");
        }
    });
}

function HidePop() {
    $('#userModal').modal('hide');
}

function ClearTextBox() {
    $('#CountryName').val('');
}

function Delete(countryId) {
    if (confirm('Are You Sure, You want to delete the Country? ')) {
        $.ajax({
            url: '/CountryManager/Delete?id=' + countryId,
            success: function (response) {
                if (response.success) {
                    alert('Country Deleted SuccessFully!');
                    showCountry();
                }
                else {
                    alert('Country not Deleted: ' + response.message);
                }
            },
            error: function () {
                alert("Country not Deleted");
            }
        });
    }
}

function Edit(countryId) {
    $('span').remove(); 
    $.ajax({
        url: '/CountryManager/Edit?id=' + countryId,
        type: 'GET',
        contentType: 'application/x-www-form-urlencoded;charset=utf-8',
        dataType: 'json',
        success: function (response) {
            $('#userModal').modal('show');
            $('#CountryId').val(response.countryId);
            $('#CountryName').val(response.countryName);

            $('#AddData').hide();
            $('#UpdateData').show();
        },
        error: function () {
            alert("Country not Found");
        }
    });
}

function UpdateCountry() {
    giveErrorCountry();
    var objData = {
        CountryId: $('#CountryId').val(),
        CountryName: $('#CountryName').val()
    };
    $.ajax({
        url: '/CountryManager/EditCountry',
        type: 'POST',
        data: objData,
        contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                alert('Country Updated Successfully');
                ClearTextBox();
                showCountry();
                HidePop();
            }
            else {
                alert('Country not Updated' + response.message);
            }
        },
        error: function () {
            alert("Country can't Updated");
        }
    });
}
