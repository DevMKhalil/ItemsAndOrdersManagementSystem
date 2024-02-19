
    document.getElementById('addItemDetailButton').addEventListener('click', function () {
        addNewItemDetailRow();
        });

    function addNewItemDetailRow() {
            var container = document.getElementById('itemDetailsContainer');
    var childLength = container.getElementsByClassName('col-md-8').length;
    var newRow = document.createElement('div');
    newRow.className = 'col-md-8';
    newRow.innerHTML = getNewItemDetailRowHtml(childLength);
    container.insertBefore(newRow, document.getElementById('addItemDetailButton'));
    updateIndexes();
        }

    function removeItemDetailRow(button) {
            var rowToRemove = button.closest('.col-md-8');
    rowToRemove.remove();
    updateIndexes();
        }

    function updateIndexes() {
            var rows = document.querySelectorAll('.col-md-8');
    rows.forEach(function (row, index) {
                var selects = row.querySelectorAll('select');
    selects.forEach(function (select) {
                    var name = select.getAttribute('name');
    var newName = name.replace(/\[\d+\]/, '[' + index + ']');
    select.setAttribute('name', newName);
                });
            });
        }

    function getNewItemDetailRowHtml(index) {
            return `
    <div class="row">
        <div class="col-md-6">
            <select name="Order.OrderItemsDtoList[${index}]" id="Order_OrderItemsDtoList_${index}" class="form-control form-select">
                <option selected disabled value="">--Items--</option>
                @foreach (var item in Model.ItemList)
                {
                    <option value="@item.Value">@item.Text</option>
                }
            </select>
        </div>
        <div class="col-md-2">
            <button type="button" onclick="removeItemDetailRow(this)" class="btn btn-danger">Delete</button>
        </div>
    </div>`;
        }
