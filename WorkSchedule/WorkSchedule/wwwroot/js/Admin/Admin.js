rolesCount = Number.parseInt(document.getElementById("AdminEdit").getAttribute("rolesCount"));
claimsCount = Number.parseInt(document.getElementById("AdminEdit").getAttribute("claimsCount"));

$(document).ready(function () {
    $('#RolesSelect').change(function () {
        getRolesBlock();
        $('#RolesSelect').val(0);
    });

    function getRolesBlock() {
        var trRoleBlock = '<tr><td><input readonly id="RolesEnumerable_' + rolesCount + '__." readonly name="RolesEnumerable[' + rolesCount + ']" class="form-control" value="' + $('#RolesSelect').val() + '" /></td></tr>';
        $('#rolesTable tbody tr:last').after(trRoleBlock);
        rolesCount++;
    }
    function getClaimsBlock() {
        var claimsValues = [];
        claimsValues = JSON.parse(document.getElementById('AdminEdit').getAttribute('Managers'));
        var trLeadClaimBlock = '<tr>\
                         <td><input id="ClaimsEnumerable_'+ claimsCount + '__Type" name="ClaimsEnumerable[' + claimsCount + '].Type" class="form-control" value="' + $('#SelectClaim').val() + '" readonly/></td>\
                         <td><select required id="ClaimsEnumerable_'+ claimsCount + '__Value" name="ClaimsEnumerable[' + claimsCount + '].Value" class="form-control"> <option></option>';
        for (var i = 0; i < claimsValues.length; i++) {
            trLeadClaimBlock += '<option value=' + claimsValues[i]+ '>' + claimsValues[i] + '</option>';
        }
        trLeadClaimBlock += '</select></td></tr>';

        $('#claimsTable tbody tr:last').after(trLeadClaimBlock);
        claimsCount++;
    }
    $('#SelectClaim').change(function () {
        getClaimsBlock();
        $('#SelectClaim').val(0);

    });
    $('#RemoveClaim').click(function () {
        if (claimsCount > 0) {
            $('#claimsTable tbody tr:last').remove();
            claimsCount--;
        }
    });
    $('#RemoveRole').click(function () {
        if (rolesCount > 0) {
            $('#rolesTable tbody tr:last').remove();
            rolesCount--;
        }
    });
});