using System;
using Application.CQS.Room.Input;
using Domain;
using Domain.Entities;

namespace Application.CQS.Room.Command
{
    public class UpdateRoomCommand
    {
        private IEntityRepository<RoomEntity> RoomRepository { get; }

        public UpdateRoomCommand(IEntityRepository<RoomEntity> roomRepository)
        {
            RoomRepository = roomRepository;
        }

        public void Execute(Guid roomId, RoomInput input)
        {
            var room = RoomRepository.Get(roomId);

            room.Cost = (int) input.Cost;
            room.RoomType = input.RoomType;
        }
    }
}