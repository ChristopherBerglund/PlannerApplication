


----------------- To-Do ----------------
1. When event is full, request for que? 
2. Sort event on Adult or Kids? (Btn on landingPage?)
3. Min/max age for event?
4. Constraint information view, when click modal pops up with further information on event? 
5. Groups


------------------ Done ----------------





------------------ Bugs ----------------
1. User can`t join event except if started an own event first (???)





---------------- Thoughts --------------
1. Kids age? (5-10? 10-15?)




















    <li class="list-group-item"><strong>Ansvarig: </strong> <a class="" type="submit" asp-action="" asp-route-id="" data-toggle="modal" data-target="#ModalProfile @b">@activity.User.firstName</a></li>







                                    
                                    <!-- Modal for User.Profile-->
                                    <div class="modal fade" id="ModalProfile @b" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel1" aria-hidden="true">
                                      <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h4 class="modal-title">Deltagare</h4>
                                            </div>
                                            <div class="modal-body">
                                               <div class="row justify-content-center">
                                                    <div class="card text">
                                                        <div class="justify-content-center align-center">
                                                            <img class="mx-auto d-block mt-4" src="@activity.User.standardPicture" alt="@activity.User.firstName" style="width:30%; height:25%; border-radius: 50%;">
                                                            <h1 class="text-center">@activity.User.firstName @activity.User.lastName</h1>
                                                            <hr />
                                                            <div class="ml-3">
                                                                <h5>Det här är @activity.User.firstName:</h5>
                                                                <p>@activity.User.aboutMe </p>

                                                                <h5>Favoriter:</h5>
                                                                <ul>
                                                                    <li>@activity.User.tagOneID</li>
                                                                    <li>@activity.User.tagTwoID</li>
                                                                    <li>@activity.User.tagThreeID</li>
                                                                </ul>
                                                                <p><strong>Ålder:</strong> @activity.User.Age</p>
                                                                <p><strong>Mail:</strong> @activity.User.Email</p>
                                                                <p><strong>Telefon:</strong> @activity.User.Phone</p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-danger" data-dismiss="modal">Stäng</button>
                                        </div>
                                    </div>
                                    </div>
                        
                        <!-- END !-->