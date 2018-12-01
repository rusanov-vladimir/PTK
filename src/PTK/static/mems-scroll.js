  function initInifiniteScroll () {
    $('.container').infiniteScroll({
        // options
        path: '/mems?page={{#}}',
        append: '.container .entry',
        history: false,
      });
  }

  function deleteMem(el) {
    var target = $(el).attr("data-href");
    var xhr = new XMLHttpRequest();
    xhr.open("DELETE", target, true);
    xhr.setRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
    xhr.onload = function () {
      window.location.reload(false);
    }
    xhr.send(null);
  }

  document.addEventListener('DOMContentLoaded', function () {
    initInifiniteScroll();
  });