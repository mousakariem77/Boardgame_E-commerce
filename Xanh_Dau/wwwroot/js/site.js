// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Lấy các phần tử cần thiết
const descriptionTab = document.getElementById('descriptionTab');
const reviewTab = document.getElementById('reviewTab');
const descriptionContent = document.getElementById('descriptionContent');
const reviewContent = document.getElementById('reviewContent');

// Chuyển đổi giữa các tab
function switchTab(activeTab, inactiveTab, activeContent, inactiveContent) {
    // Kích hoạt tab và nội dung tương ứng
    activeTab.classList.add('text-green-600', 'border-green-600');
    activeTab.classList.remove('text-gray-600', 'border-transparent');
    activeContent.classList.remove('hidden');

    // Hủy kích hoạt tab và nội dung không chọn
    inactiveTab.classList.add('text-gray-600', 'border-transparent');
    inactiveTab.classList.remove('text-green-600', 'border-green-600');
    inactiveContent.classList.add('hidden');
}

// Lắng nghe sự kiện click trên các tab
descriptionTab.addEventListener('click', () => switchTab(descriptionTab, reviewTab, descriptionContent, reviewContent));
reviewTab.addEventListener('click', () => switchTab(reviewTab, descriptionTab, reviewContent, descriptionContent));