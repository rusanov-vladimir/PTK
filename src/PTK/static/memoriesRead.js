$('.container').infiniteScroll({
    // options
    path: '/memories?page={{#}}',
    append: '.container .entry',
    history: false,
  });