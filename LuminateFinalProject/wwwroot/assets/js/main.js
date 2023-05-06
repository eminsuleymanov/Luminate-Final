$( function() {
    const sliderRange = $( "#slider-range" );
    const minSpan = $( "#min-value" );
    const maxSpan = $( "#max-value" );
    
    sliderRange.slider({
      range: true,
      min: 0,
      max: 8500,
      values: [ 2000, 3000 ],
      slide: function( event, ui ) {
        minSpan.text(ui.values[ 0 ].toFixed(2));
        maxSpan.text(ui.values[ 1 ].toFixed(2));
      }
    });
    
    minSpan.text(sliderRange.slider( "values", 0 ).toFixed(2));
    maxSpan.text(sliderRange.slider( "values", 1 ).toFixed(2));
});

  let dashboardTabLi = document.querySelectorAll('.tab-li');
  let dashboardContent = document.querySelectorAll('.my-account-content');

  dashboardTabLi.forEach((item,index) =>{
    item.addEventListener('click',()=>{
      dashboardTabLi.forEach(item => {
        item.classList.remove('tab-li-active')
      })
      item.classList.add('tab-li-active');
      dashboardContent.forEach(item => {
        item.classList.remove("my-account-content-active");
      })
      dashboardContent[index].classList.add('my-account-content-active');
  
    })
  })

  let mainImage = document.getElementById('MainImg');
  let detailImages = document.querySelectorAll('.detail-image')

  function ChangeImage(detailImages){
    mainImage.src = detailImages.src;
  }
  
  $(".product-main-image")
  .on("mouseover", function() {
    $(this)
      .children("#MainImg")
      .css({ transform: "scale(" + $(this).attr("data-scale") + ")" });
  })
  .on("mouseout", function() {
    $(this)
      .children("#MainImg")
      .css({ transform: "scale(1)" });
  })
  .on("mousemove", function(e) {
    $(this)
      .children("#MainImg")
      .css({
        "transform-origin":
          ((e.pageX - $(this).offset().left) / $(this).width()) * 100 +
          "% " +
          ((e.pageY - $(this).offset().top) / $(this).height()) * 100 +
          "%"
      });
  });

  let productTabLi = document.querySelectorAll('.product-tab-li');
  let productTabContent = document.querySelectorAll('.product-tab-content');

  productTabLi.forEach((item,index)=>{
    item.addEventListener('click',()=>{
      productTabLi.forEach(item=>{
        item.classList.remove('product-tab-li-active')
      })
      item.classList.add('product-tab-li-active');
      productTabContent.forEach(item => {
        item.classList.remove("product-tab-content-active");
      })
      productTabContent[index].classList.add('product-tab-content-active');
  
    })
  })

  let loginTabLi = document.querySelectorAll('.log-reg-tab-li');
  let loginTabContent = document.querySelectorAll('.login-register-tab-content');

  loginTabLi.forEach((item,index)=>{
    item.addEventListener('click',()=>{
      loginTabLi.forEach(item=>{
        item.classList.remove('log-reg-tab-li-active')
      })
      item.classList.add('log-reg-tab-li-active');
      loginTabContent.forEach(item => {
        item.classList.remove('login-register-tab-content-active');
      })
      loginTabContent[index].classList.add('login-register-tab-content-active');
  
    })
  })

 const miniCart = document.querySelector('#MiniCart');
 const miniCartBtn = document.querySelector('.minicart-btn')
 const miniCartCloseBtn = document.querySelector('.minicart-close-btn');

 miniCartBtn.addEventListener('click',()=>{
  miniCart.className = 'active';
 })
  miniCartCloseBtn.addEventListener('click', ()=>{
    miniCart.classList.remove('active');
  })


function plusQtyBtn(){
  const plusBtn = document.querySelectorAll('.plus-qty');
  let inputValueAll = document.querySelectorAll('.product-cart-count')
  let count = 1;
  plusBtn.forEach((item,index) => {
    item.addEventListener('click',()=>{
      let inputValue = Number(inputValueAll[index].value);
      inputValueAll[index].value = inputValue +1;
    })
  });
}
plusQtyBtn();

function minusQtyBtn(){
  const minusBtn = document.querySelectorAll('.minus-qty');
  const inputValueAll = document.querySelectorAll('.product-cart-count');
  minusBtn.forEach((item, index) => {
    item.addEventListener('click', () => {
      let inputValue = Number(inputValueAll[index].value);
      if (inputValue > 1) {
        inputValueAll[index].value = inputValue -1;
      }
    });
  });
}
minusQtyBtn();





