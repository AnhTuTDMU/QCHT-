/*function loadSubCategories(categoryId) {
    $.ajax({
        url: '@Url.Action("GetSubCategories", "Category", new { area = "Admin" })',
        type: 'GET',
        data: { categoryId: categoryId },
        success: function (result) {
            $('#subCategoriesContainer').html(result);
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', status, error);
            console.error('Response:', xhr.responseText);
            alert('Có lỗi xảy ra khi tải dữ liệu.');
        }
    });
}

function editSubCategory(subCategoryId) {
    $.ajax({
        url: '@Url.Action("GetSubCategory", "Category", new { area = "Admin" })',
        type: 'GET',
        data: { id: subCategoryId },
        success: function (result) {
            // Điền dữ liệu vào các trường trong modal
            $('#subCategoryId').val(result.subCategoryId);
            $('#subCategoryName').val(result.subCategoryName);
            // Hiển thị modal
            $('#subCategoryModal').modal('show');
        },
        error: function (xhr, status, error) {
            console.error('AJAX Error:', status, error);
            console.error('Response:', xhr.responseText);
            alert('Có lỗi xảy ra khi tải dữ liệu.');
        }
    });
}
$(document).ready(function () {
    $('#subCategoryForm').submit(function (e) {
        e.preventDefault(); // Ngăn chặn hành vi mặc định của form

        var subCategoryId = $('#subCategoryId').val();
        var subCategoryName = $('#subCategoryName').val();

        $.ajax({
            url: '/Admin/Category/SaveSubCategory', // Đảm bảo URL chính xác
            type: 'POST',
            dataType: 'json',
            data: {
                subCategoryId: subCategoryId,
                subCategoryName: subCategoryName
            },
            success: function (response) {
                if (response.success) {
                    // Tải lại danh sách sub-categories hoặc làm mới dữ liệu
                    var categoryId = getCurrentCategoryId(); // Xác định ID category hiện tại
                    loadSubCategories(categoryId);

                    // Đóng modal
                    $('#subCategoryModal').modal('hide');
                } else {
                    alert('Có lỗi xảy ra khi lưu dữ liệu.');
                }
            },
            error: function (xhr, status, error) {
                console.error('AJAX Error:', status, error);
                console.error('Response:', xhr.responseText);
                alert('Có lỗi xảy ra khi lưu dữ liệu.');
            }
        });
    });

    // Các hàm khác, như loadSubCategories, viewSubCategoryDetails, v.v.
});


function deleteSubCategory(subCategoryId) {
    if (confirm('Bạn có chắc chắn muốn xóa mục này không?')) {
        $.ajax({
            url: '@Url.Action("DeleteSubCategory", "Category", new { area = "Admin" })',
            type: 'POST',
            data: { id: subCategoryId },
            success: function () {
                // Tải lại danh sách sub-categories sau khi xóa thành công
                var categoryId = getCurrentCategoryId(); // Xác định ID category hiện tại
                loadSubCategories(categoryId);
            },
            error: function (xhr, status, error) {
                console.error('AJAX Error:', status, error);
                console.error('Response:', xhr.responseText);
                alert('Có lỗi xảy ra khi xóa dữ liệu.');
            }
        });
    }
}



function getCurrentCategoryId() {
    // Xác định ID của category hiện tại (cần implement)
    return 1; // Ví dụ trả về ID 1
}*/