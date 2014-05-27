namespace SnakeGame
{
    using System;

    public struct Position
    {
        public Position(int column,int row)
        {
            this.Column = column;
            this.Row = row;
        }

        public int Row;/*{ get; set; }*/
        public int Column;/*{ get; set; }*/

        //TODO
        public override bool Equals(object obj)
        {
            //Position position = obj as Position;

            //if (this.Row == position.Row
            //    && this.Column == position.Column)
            //{
            //    return true;
            //}
            //return false;

            return base.Equals(obj);
        }

        //TODO
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Position firstElement, Position secondElement)
        {
            if (firstElement.Row == secondElement.Row
                && firstElement.Column == secondElement.Column)
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(Position firstElement, Position secondElement)
        {
            return !(firstElement == secondElement);
        }
    }
}