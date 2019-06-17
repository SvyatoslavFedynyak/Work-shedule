var counter = Number.parseInt(document.getElementById('ScheduleCreate').getAttribute('counter'));
var WorkerTypes = JSON.parse(document.getElementById('ScheduleCreate').getAttribute('WorkerTypes'));
var SelectedWorkers = JSON.parse(document.getElementById('ScheduleCreate').getAttribute('SelectedWorkers'));
$(document).ready(function () {

    $('input[name="Schedule.From"]').on('change', function () {
        $('input[name="Schedule.To"]').attr('min', $('input[name="Schedule.From"]').val());
        if ($('input[name="Schedule.From"]').val() > $('input[name="Schedule.To"]').val()) {
            $('input[name="Schedule.To"]').val($('input[name="Schedule.From"]').val());
        }
        for (var i = 0; i < counter; i++) {
            $('input[name="TaskLines[' + i + '].Day"]').attr('min', $('input[name="Schedule.From"]').val()).attr('max', $('input[name="Schedule.To"]').val());
        }
    });

    function updateMinMaxDate() {
        for (var i = 0; i < counter; i++) {
            $('input[name="TaskLines[' + i + '].Day"]').attr('min', $('input[name="Schedule.From"]').val()).attr('max', $('input[name="Schedule.To"]').val());
        }
    }

    $('input[name="Schedule.To"]').on('change', function () {
        updateMinMaxDate();
    });

    for (let i = 0; i < counter; i++) {
        TypeChanged($('select[name="TaskLines[' + i + '].WorkerTypeID"]'), true);
    }

    function generateBlock() {

        var block = '<tr rownum="' + counter + '"><td><div style="height:20px;width:20px;background-color:dodgerblue"></div></td><td><select required onchange="TypeChanged($(this),false)" id="TaskLines_' + counter + '__.WorkerTypeID" name="TaskLines[' + counter + '].WorkerTypeID" class="form-control"><option></option>';
        for (let i = 0; i < WorkerTypes.length; i++) {
            block = block + '<option value="' + WorkerTypes[i].WorkerTypeID + '">' + WorkerTypes[i].WorkerTypeName + '</option>';
        }
        block = block + '</select></td><td><select  id="TaskLines_' + counter + '__.WorkerID" name="TaskLines[' + counter + '].WorkerID" class="form-control">';
        block = block + '</select></td>';
        block = block + '<td><input required id="TaskLines_' + counter + '__.From" name="TaskLines[' + counter + '].From" type="time" class="form-control" /></td>';
        block = block + '<td><input required id="TaskLines_' + counter + '__.To" name="TaskLines[' + counter + '].To" type="time" class="form-control" /></td>';
        block = block + '<td><input required id="TaskLines_' + counter + '__.Day" name="TaskLines[' + counter + '].Day" type="date" class="form-control" /></td>';
        block = block + '<td><button type="button" name="Delete" onclick="forRemove($(this))" class="close" aria-label="Close"><span aria-hidden="true">&times;</span></button></td></tr>';
        $('#TaskL tbody tr:last').after(block);
        counter++;
    }

    $('#addNewLine').click(function () {
        generateBlock();
        updateMinMaxDate();
    });
});
function forRemove(button) {

    var currentCounter = $(button).parent().parent().attr('rownum');
    $(button).parent().parent().remove();
    counter--;
    for (let i = Number.parseInt(currentCounter) + 1; i <= counter; i++) {
        var place = $("tr[rownum='" + i.toString() + "']");
        place.attr("rownum", i - 1);

        var children1 = place.children().children("input[id^='TaskLines']");
        var children2 = place.children().children("select[id^='TaskLines']");
        children1.toArray().forEach(function (child) {
            child.setAttribute("name", child.getAttribute("name").replace("[" + i.toString() + "]", "[" + (i - 1) + "]"));
            child.setAttribute("id", child.getAttribute("id").replace("_" + i.toString(), "_" + (i - 1)));
        });
        children2.toArray().forEach(function (child) {
            child.setAttribute("name", child.getAttribute("name").replace("[" + i.toString() + "]", "[" + (i - 1) + "]"));
            child.setAttribute("id", child.getAttribute("id").replace("_" + i.toString(), "_" + (i - 1)));
        });
    }
}
function TypeChanged(select, initial) {
    var value = $(select).val();
    var currentIndex = Number.parseInt($(select).parent().parent().attr('rownum'));
    if (value != "") {
        $.ajax({
            type: "GET",
            url: "http://localhost:64332/Tasks/GetWorkers",
            contentType: "application/json",
            dataType: "json",
            data: { "role": value },
            success: function (response) {
                $('select[name="TaskLines[' + currentIndex + '].WorkerID"] option').remove();
                $('select[name="TaskLines[' + currentIndex + '].WorkerID"]').append($('<option></option>'));
                response.forEach(function (el) {
                    if (initial) {
                        if (el == SelectedWorkers[currentIndex]) {
                            $('select[name="TaskLines[' + currentIndex + '].WorkerID"]').append($('<option selected="true" value="' + el + '">' + el + '</option>'));
                        } else {
                            $('select[name="TaskLines[' + currentIndex + '].WorkerID"]').append($('<option value="' + el + '">' + el + '</option>'));
                        }
                    } else {
                        $('select[name="TaskLines[' + currentIndex + '].WorkerID"]').append($('<option value="' + el + '">' + el + '</option>'));
                    }
                });

            },
            failure: function (response) {
                alert(response);
            }
        });
    } else {
        $('select[name="TaskLines[' + currentIndex + '].WorkerID"] option').remove();
    }
}