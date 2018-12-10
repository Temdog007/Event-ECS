define(function()
{
    class Queue
    {
        constructor()
        {
            this._list = [];
            this._offset = 0;
        }

        get length()
        {
            return (this._list.length - this._offset);
        }

        get isEmpty()
        {
            return this._list.length == 0;
        }

        enqueue(value)
        {
            this._list.push(value);
        }

        dequeue()
        {
            if(this.isEmpty){return undefined;}

            var item = this._list[this._offset];

            if(++this._offset * 2 > this._list.length)
            {
                this._list = this._list.slice(this._offset);
                this._offset = 0;
            }
            
            return item;
        }

        peek()
        {
            return this._list.length > 0 ? this._list[this._offset] : undefined;
        }

        *[Symbol.iterator]()
        {
            for(var i = this._offset; i < this._list.length; ++i)
            {
                yield this._offset[i];
            }
        }
    }

    return Queue;
})