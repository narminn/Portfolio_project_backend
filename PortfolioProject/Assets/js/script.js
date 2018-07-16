
$(function(){
  $('.homepage').masonry({
    // options
    itemSelector : '.item',
    columnWidth : 120
  });
});
$(function(){

  $('.thumbnail').viewbox();


  (function(){
    var vb = $('.popup-link').viewbox();
    $('.popup-open-button').click(function(){
      vb.trigger('viewbox.open');
    });
    $('.close-button').click(function(){
      vb.trigger('viewbox.close');
    });
  })();

});
function openNav() {
    document.getElementById("MySidenav").style.transform = 'translate3d(0, 0, 0)';
    document.getElementById("Main").style.transform = 'translate3d(250px, 0, 0)';
    document.getElementsByClassName("_full_opasity")[0].style.zIndex = '2';
    document.getElementsByClassName("_full_opasity")[0].style.backgroundColor = 'rgba(0,0,0,0.5)';

}

/* Set the width of the side navigation to 0 and the left margin of the page content to 0, and the background color of body to white */
function closeNav() {
    document.getElementById("MySidenav").style.transform = 'translate3d(-250px, 0, 0)';;
    document.getElementById("Main").style.transform = 'translate3d(0, 0, 0)';
    document.getElementsByClassName("_full_opasity")[0].style.backgroundColor = 'rgba(0, 0, 0, 0)';
    document.getElementsByClassName("_full_opasity")[0].style.zIndex = '-1';

}

$('#exampleModal').on('show.bs.modal', function (event) {
  var button = $(event.relatedTarget) // Button that triggered the modal
  var recipient = button.data('whatever') // Extract info from data-* attributes
  // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
  // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
  var modal = $(this)
  modal.find('.modal-title').text('New message to ' + recipient)
  modal.find('.modal-body input').val(recipient)
})
