var loader = ``;
loader = `<div class="overlay" id="Spinner">
    <div class="overlay__inner">
        <div class="overlay__content"><span class="spinner"></span></div>
    </div>
</div>`;

function removeSpinner() {
    $('.loader').remove();
    $('body').css('background-color', '');
    $('body').css('opacity', '');
    $('body').css('pointer-events', '');
}

function AddSpinner() {

    $('body').append('<span class="loader" style="z-index:9999;position:absolute;left:50%;top:25%;"></span>');
    $('body').css('background-color', '#F5F5F5');
    $('body').css('opacity', '50%');
    $('body').css('pointer-events', 'none');
};
function fnChangeActiveClass(p_this)//id)
{
    $('.open').removeClass('open');
    if ($(p_this).hasClass('open')) {
        $('.open').removeClass('open');
    }
    else {
        $(p_this).addClass('open');

    }

}

$('.tooltip-r').tooltip();

function CheckMandatory() {
    var textboxcounter = 0;
    var selectboxcounter = 0;
    var passwordboxcounter = 0;
    var filecounter = 0;
    $('input[type=text]').each(function () {
        if ($(this).hasClass('mandatory')) {
            var currinput = $(this).val();
            var currid = $(this).attr('id');
            if (currinput == "") {
                textboxcounter = 1;
                $('#' + currid + '').css('border-color', 'red');
            }
            else {
                $('#' + currid + '').css('border-color', '#CACACA');
            }
        }
    });
    $('textarea').each(function () {
        if ($(this).hasClass('mandatory')) {
            var currinput = $(this).val();
            var currid = $(this).attr('id');
            if (currinput == "") {
                textboxcounter = 1;
                $('#' + currid + '').css('border-color', 'red');
            }
            else {
                $('#' + currid + '').css('border-color', '#CACACA');
            }
        }
    });
    $('input[type=password]').each(function () {
        if ($(this).hasClass('mandatory')) {
            var currinput = $(this).val();
            var currid = $(this).attr('id');
            if (currinput == "") {
                passwordboxcounter = 1;

                $('#' + currid + '').css('border-color', 'red');
            }
            else {
                $('#' + currid + '').css('border-color', '#CACACA');
            }
        }
    });

    $('input[type=file]').each(function () {
        if ($(this).hasClass('mandatory')) {
            var currinput = $(this).val();
            var currid = $(this).attr('id');
            if (currinput == "") {
                filecounter = 1;

                $('#' + currid + '').css('border-color', 'red');
            }
            else {
                $('#' + currid + '').css('border-color', '#CACACA');
            }
        }
    });

    $('select').each(function () {
        if ($(this).hasClass('mandatory')) {
            var curInput = $(this).val();
            var curId = $(this).attr('id');
            if (curInput == "0") {
                selectboxcounter = 1;
                $('#' + curId + '').css('border-color', 'red');
            } else {
                $('#' + curId + '').css('border-color', '#CACACA');
            }
        }

    });

    if (textboxcounter == 1 || selectboxcounter == 1 || passwordboxcounter == 1 || filecounter == 1) {
        textboxcounter = 0;//textboxcounterEdit = 0;
        selectboxcounter = 0;// selectboxcounterEdit = 0;
        passwordboxcounter = 0;
        filecounter = 0;
        //swal("Info!!", "Kindly fill mandatory fields", "warning");
        //Toast.fire({ icon: 'warning', title: 'Kindly Fill Mandatory Fields' });
        toastr.warning('Kindly Fill Mandatory Fields');
        return false;
    }
    return true;
}

function NumericValueOnly() {
    //$('.onlynumeric').on('keypress', function (e) {
    //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
    //        return false;
    //    }
    //});
}

function datePicker() {
    $('.datepicker').datepicker({
        dateFormat: 'dd-mm-yy',
        startDate: new Date('01/01/1970'),
        //endDate: new Date()
        maxDate: '0'
    }).on('changeDate', function (e) {
        $(this).datepicker('hide');
    });
}

function ManageNavMenuColor()
{
    $('.side-menu li').each(function () {
        if ($(this).hasClass('is-expanded')) {
            $(this).removeClass('is-expanded')
        }
        if ($(this).find('a').hasClass('active')) {
            $(this).find('a').removeClass('active')
        }
    })

}


$(document).ready(function () {
    //datePicker();
    ManageNavMenuColor();
    $('.clickMenu').click(function () {
        //fnChangeActiveClass($(this)[0].attr("id"));
        fnChangeActiveClass($(this)[0]);
    });
    $('.clickMenu').on('mouseenter', function () {
        /*var p_this = $(this)[0];
        $('.open').removeClass('open');
        $(p_this).addClass('open')
        $(p_this).parent().addClass('open')
        */

    });


    function preventBack() { window.history.forward(); }
    setTimeout("preventBack()", 0);
    window.onunload = function () { null };


    $('#a_UserPref').click(function () {
        $('#myPrefModal').modal('toggle');
    })
    $('#btnPrefSubmit').click(function () {
        var rolecode = "";
        var defaultrole = "";
        $('#gvPrefUserRoleMapping tbody tr').each(function () {
            rolecode = $(this).find('td:eq(2)').text();
            defaultrole = $(this).find('td:eq(3) input');
            defaultrole = $(defaultrole)[0].checked
            if (defaultrole == true) {
                return false;
            }
        })


        $.ajax({

            // contentType: "application/json; charset=utf-8",
            url: "/Home/ChangeRolePreference",
            data: { RoleCode: rolecode, DefaultRole: defaultrole},
            type: "post",
            success: function (response) {
                if (response.message == "Success") {
                    $('#myPrefModal').modal('toggle');
                    //toastr.success('Preference updated successfully');
                    Swal.fire({
                        title: 'Preference updated successfully',
                        showDenyButton: false,
                        showCancelButton: false,
                        confirmButtonText: `Ok`,
                        allowOutsideClick:false,

                        customClass: {
                            cancelButton: 'order-1 right-gap',
                            confirmButton: 'order-2',
                            denyButton: 'order-3',
                        }
                    }).then((result) => {
                        if (result.isConfirmed) {
                            location.reload();
                        }
                        
                    });
                }
                else {
                    toastr.error('Error occurred while updating preference');
                }
            },
            error: function (response) {
                toastr.error('Error occurred while updating preference');
            }
        });
        
    })

    
});

//16062025