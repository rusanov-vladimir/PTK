$('.container').infiniteScroll({
    // options
    path: '/search?page={{#}}',
    append: '.container .entry',
    history: false,
  });