
//Function to loaddata to Select Option
$(function () {  

    AjaxCall('https://localhost:44325/api/Province/GetAllProvince/', null).done(function (response) {  
        if (response.length > 0) {  
            $('#province').html('');  
            var options = '';  
            //options += '<option value="Select">Chọn tỉnh/thành phố</option>';  
            for (var i = 0; i < response.length; i++) {  
                options += '<option value="' + response[i].Id + '">' + response[i].Name + '</option>';  
            }  
            $('#province').append(options); 
        }  
    }).fail(function (error) {  
        alert(error.StatusText);  
    });  

    

    $('#province').on("change", function () {  
            var province = $('#province').val();  
            var obj = { province: province };  
            AjaxCall('https://localhost:44325/api/District/GetDistrictByProvinceId/'+province, JSON.stringify(obj.province)).done(function (response) {  
                if (response.length > 0) {  
                    $('#district').html('');  
                    var options = '';  
                    options += '<option value="Select">Chọn quận/huyện</option>';  
                    for (var i = 0; i < response.length; i++) {  
                        options += '<option value="' + response[i].Id + '">' + response[i].Type + " " + response[i].Name + '</option>';  
                    }  
                    $('#district').append(options);  
                }  
            }).fail(function (error) {  
                alert(error.StatusText);  
            });  
    });  

    $('#district').on("change", function () {  
            var district = $('#district').val();  
            var obj = { district: district };  
            AjaxCall('https://localhost:44325/api/Ward/GetWardByDistrictId/'+district, JSON.stringify(obj)).done(function (response) {  
                if (response.length > 0) {  
                    $('#ward').html('');  
                    var options = '';  
                    //options += '<option value="Select">Chọn phường/xã</option>';  
                    for (var i = 0; i < response.length; i++) {  
                        options += '<option value="' + response[i].Id + '">' + response[i].Type + " " + response[i].Name + '</option>';  
                    }  
                    $('#ward').append(options);  
                }  
            }).fail(function (error) {  
                alert(error.StatusText);  
            });  
    });  
});
    
      

function AjaxCall(url, data, type) {  
    return $.ajax({  
        url: url,  
        type: type ? type : 'GET',  
        data: data,  
        contentType: 'application/json'  
    });  
}