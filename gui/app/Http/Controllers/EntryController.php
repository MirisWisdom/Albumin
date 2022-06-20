<?php

namespace App\Http\Controllers;

use App\Models\Entry;
use App\Models\Identifier;
use App\Models\Record;
use App\Models\Source;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;

class EntryController extends Controller
{
    /**
     * Store a newly created resource in storage.
     *
     * @param Source $source
     * @param Record $record
     * @param Request $request
     * @return RedirectResponse
     */
    public function store(Source $source, Record $record, Request $request): RedirectResponse
    {
        Entry::store($source, $record, $request);

        return back();
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param Source $source
     * @param Record $record
     * @param Entry $entry
     * @return RedirectResponse
     */
    public function destroy(Source $source, Record $record, Entry $entry): RedirectResponse
    {
        if ($entry->identifier == Identifier::infer()) {
            $entry->delete();

            info('Deleted Entry from the database.', [
                'entry'  => $entry->id,
                'record' => $record->id,
                'source' => $source->id
            ]);
        }

        return back();
    }
}
