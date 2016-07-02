# Sagen

Sagen is my attempt at making a text-to-speech engine aimed at .NET developers who don't have thousands of dollars at their disposal to license a commercial speech synthesis solution.

Aside from often being prohibitively expensive, commercial TTS technologies are frequently restrictive in their available customization options for voices, voice parameters, and context-sensitive vocal qualities like intonation, stress, and timbre which are necessary to convey emotion and intent. Concatenative synthesizers and other similar "realistic" synthesizers based on databases of recorded samples leave a large memory footprint, are of relatively poor performance, and require each voice to be installed separately.

My goal with this project is to offer the most customization options, flexibility, and performance in a formant synthesizer of decent quality. For free.

Here is a short list of major features that will be supported:
* Text-to-speech based on [formant synthesis](https://en.wikipedia.org/wiki/Speech_synthesis#Formant_synthesis) and physically-based vocal filtering
* Plentiful parameters for tuning how voices sound (age, sex, vocal force, hoarseness, etc...)
* Support for sending speech directly to a WAV file or stream
* Multiple options for sample format and rate
* Support for both [IPA](https://en.wikipedia.org/wiki/International_Phonetic_Alphabet) and [X-SAMPA](https://en.wikipedia.org/wiki/X-SAMPA) pronunciation dictionaries
* Multiple language support (English and German are currently prioritized)
* Singing

It is currently a heavy work-in-progress, and I welcome your input and/or contributions.

## Licensing

This project is made available under the MIT License and is completely free for anyone to use, for any purpose, without the burdens of licensing costs or royalties.
