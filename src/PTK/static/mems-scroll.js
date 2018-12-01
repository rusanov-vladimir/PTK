$('.container').infiniteScroll({
    // options
    path: '/mems?page={{#}}',
    append: '.container .entry',
    history: false,
  });